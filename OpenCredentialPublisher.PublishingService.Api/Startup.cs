using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using OpenCredentialPublisher.PublishingService.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace OpenCredentialPublisher.PublishingService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IdentityModelEventSource.ShowPII = true;

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            string domain = Configuration["AppBaseUri"];

            services.AddOptions();

            var keyVaultSection = Configuration.GetSection(AzureKeyVaultOptions.Section);
            services.Configure<AzureKeyVaultOptions>(keyVaultSection);
            services.Configure<AzureBlobOptions>(Configuration.GetSection(AzureBlobOptions.Section));
            services.Configure<AzureQueueOptions>(Configuration.GetSection(AzureQueueOptions.Section));

            services.AddMemoryCache();

            // https://stackoverflow.com/questions/44379560/how-to-enable-cors-in-asp-net-core-webapi
            // https://github.com/dotnet/aspnetcore/issues/4457
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder //.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                    });
            });

            services.AddResponseCaching();
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddAuthentication()
                 .AddJwtBearer(jwt => {
                     jwt.Authority = domain;
                     jwt.ClaimsIssuer = domain;
                     jwt.RequireHttpsMetadata = false;
                     jwt.Audience = "opencredentials";
                 });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ScopeConstants.Publisher, policy => policy.Requirements.Add(new HasScopeRequirement(ScopeConstants.Publisher, domain)));
                options.AddPolicy(ScopeConstants.Wallet, policy => policy.Requirements.Add(new HasScopeRequirement(ScopeConstants.Wallet, domain)));
            });

            services.AddScoped<IAuthorizationHandler, HasScopeHandler>();

            var idsvrConnectionString = Configuration["IdentityDbConnectionString"];
            var appConnectionString = Configuration["ApplicationDbConnectionString"];

            IIdentityServerBuilder ids = services.AddIdentityServer(options =>
                    {
                        // see http://docs.identityserver.io/en/latest/topics/add_apis.html
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.ConfigEndpoint, "~/api/configuration");
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.RegistrationEndpoint, "~/connect/register");
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.WalletConnectEndpoint, "~/api/connect-wallet");
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.CredentialsEndpoint, "~/api/credentials");
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.KeysEndpoint, "~/api/keys");
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.PublishEndpoint, "~/api/publish");
                        options.Discovery.CustomEntries.Add(DiscoveryDocumentCustomEndpointsConstants.RequestsEndpoint, "~/api/requests");

                        // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                        options.EmitStaticAudienceClaim = true;
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;
                    });

            if (Environment.IsDevelopmentOrLocalhost())
            {
                ids.AddDeveloperSigningCredential();
            }
            else
            {
                var keyVaultOptions = keyVaultSection.Get<AzureKeyVaultOptions>();
                if (keyVaultOptions.UseRoleBasedAccess)
                {
                    var certificateClient = new CertificateClient(new Uri(keyVaultOptions.KeyVaultBaseUri), new DefaultAzureCredential());
                    var azureResponse = certificateClient.DownloadCertificate(keyVaultOptions.CertificateName);
                    ids.AddSigningCredential(azureResponse.Value);
                }
                else
                {
                    var credential = new ClientSecretCredential(keyVaultOptions.AzureTenantId, keyVaultOptions.AzureAppClientId, keyVaultOptions.AzureAppClientSecret);
                    var certificateClient = new CertificateClient(new Uri(keyVaultOptions.KeyVaultBaseUri), credential);
                    var certificateResponse = certificateClient.DownloadCertificate(keyVaultOptions.CertificateName);
                    ids.AddSigningCredential(certificateResponse.Value);
                }
            }
               

            // EF client, scope, and persisted grant stores
            ids.AddOperationalStore(options =>
            {
                options.DefaultSchema = "idsvr";
                options.ConfigureDbContext = builder =>
                {
                    builder.UseSqlServer(idsvrConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                };
            });

            ids.AddConfigurationStore(options =>
            {
                options.DefaultSchema = "idsvr";
                options.ConfigureDbContext = builder =>
                {
                    builder.UseSqlServer(idsvrConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                };
            }).AddClientStore<ClientStore>();


            services.AddDbContext<OcpDbContext>(builder =>
            {
                builder.UseSqlServer(appConnectionString,
                        sql =>
                        {
                            sql.MigrationsAssembly(migrationsAssembly);

                            sql.EnableRetryOnFailure(
                                    maxRetryCount: 5,
                                    maxRetryDelay: TimeSpan.FromSeconds(2),
                                    errorNumbersToAdd: null);
                        });
            });

            services.AddMediatR(typeof(Startup));

            services.AddTransient<IFileStoreService, AzureBlobStoreService>();
            services.AddTransient<IQueueService, AzureQueueService>();
            services.AddTransient<IPublishService, PublishService>();
            services.AddTransient<IRevocationListService, RevocationListService>();
            services.AddTransient<IDynamicClientRegistrationService, DynamicClientRegistrationService>();
            services.AddTransient<IKeyStore, AzureKeyVaultDatabaseRegistryService>();
            services.AddTransient<ProofService>();

            services.AddSingleton<ICorsPolicyService>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true
                };
            });

            services.AddSignalR();
            //services.AddHostedService<RequestNotficationHostedService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OpenCredentials Publish Service API",
                    Description = "",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Keith Williams & Michael McFarren",
                        Email = string.Empty
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under License",
                        Url = new Uri("https://someplace.com/license"),
                    }
                });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"{domain.TrimEnd('/')}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {ScopeConstants.Publisher, "OCP Publisher"},
                                {ScopeConstants.Wallet, "OCP Wallet"}
                            },
                            
                        }
                    }
                });

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                c.OperationFilter<SwaggerAuthorizationOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.DocInclusionPredicate((name, api) => true);
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

        }

        public void Configure(IApplicationBuilder app)
        {
            // TODO: Remove this line once database is stable
            //InitializeDatabase(app);

            app.Use(async (ctx, next) =>
            {
                ctx.SetIdentityServerOrigin(Configuration["AppBaseUri"]);
                await next();
            });

            if (Environment.IsDevelopmentOrLocalhost())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Publish Service V1");
                //c.OAuthUsePkce();
            }); 

           

            // to access /.well-known/openid-configuration make sure UseCors appears before UseIdentityServer
            app.UseCors();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<RequestNotificationHub>("/hubs/requests");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            /*
             * Add-Migration PersistedInitialCreate -Context PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
             * Add-Migration ConfigInitialCreate -Context ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
             * Add-Migration OcpInitialCreate -Context OcpDbContext -o Data/Migrations/Application/OcpDb
            */

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<OcpDbContext>().Database.Migrate();

                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerSetup.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerSetup.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in IdentityServerSetup.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerSetup.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

    }

}
