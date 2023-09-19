using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Credentials.Clrs.v1_0.Interfaces;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using System;
using System.Linq;
using System.Reflection;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using OpenCredentialPublisher.PublishingService.Shared;

[assembly: FunctionsStartup(typeof(OpenCredentialPublisher.PublishingService.Functions.Startup))]

namespace OpenCredentialPublisher.PublishingService.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", true)
               .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
               .AddEnvironmentVariables()
               .Build();

            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddOptions();

            builder.Services.Configure<AzureKeyVaultOptions>(config.GetSection(AzureKeyVaultOptions.Section));
            builder.Services.Configure<AzureQueueOptions>(config.GetSection(AzureQueueOptions.Section));
            builder.Services.Configure<AzureBlobOptions>(config.GetSection(AzureBlobOptions.Section));

            string connectionString = config["ApplicationDbConnectionString"];
            
            builder.Services.AddDbContext<OcpDbContext>(
                options => {
                    SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString,
                        options => options.EnableRetryOnFailure(
                                   maxRetryCount: 5,
                                   maxRetryDelay: TimeSpan.FromSeconds(2),
                                   errorNumbersToAdd: null)
                        );
                });

            builder.Services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(typeof(Startup).Assembly);
            });
            builder.Services.AddHttpClient("default");
            builder.Services.AddScoped<IQueueService, AzureQueueService>();
            builder.Services.AddScoped<IFileStoreService, AzureBlobStoreService>();
            builder.Services.AddScoped<IKeyStore, AzureKeyVaultDatabaseRegistryService>();

            builder.Services.AddCommandQueryHandlers(typeof(ICommandHandler<>));
            builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        }

    }

    public static class RegisterHandlers
    {
        public static void AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
            }
        }
    } 

}
