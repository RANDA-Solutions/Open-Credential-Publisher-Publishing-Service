using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.PublishingService.Data;
using OpenCredentialPublisher.PublishingService.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace AzureKeyVault.Tests
{
    public class UnitTest1
    {
        private static void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();
            
            services.AddOptions();

            services.Configure<AzureKeyVaultOptions>(config.GetSection(AzureKeyVaultOptions.Section));
            
            services.AddDbContext<OcpDbContext>(builder =>
            {
                builder.UseSqlServer(config["ApplicationDbConnectionString"]);
            });

            services.AddSingleton<IConfiguration>(config);

        }

        [Fact]
        public async Task Test1()
        {
            
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            // create service provider
            var serviceProvider = services.BuildServiceProvider();

            var kvOptions = serviceProvider.GetService<IOptions<AzureKeyVaultOptions>>();

            var context = serviceProvider.GetService<OcpDbContext>();

            var service = new AzureKeyVaultDatabaseRegistryService(kvOptions, context);

            var keyId = Guid.NewGuid().ToString();
            var issuerId = "testIssuer";

            var keyIdent = await service.CreateKeyAsync(keyId, issuerId);

            var bundle = await service.GetKeyBundleAsync(keyId);

            Assert.Equal(keyId, bundle.KeyIdentifier.Name);

        }
    }
}
