using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(
        services =>
        {
            services.AddSingleton<IGraphClientService, GraphClientService>(
                provider => new GraphClientService(
                    clientId: AppSettingsHelper.GetSettingValue("clientId"),
                    clientSecret: AppSettingsHelper.GetSettingValue("clientSecret"),
                    tenantId: AppSettingsHelper.GetSettingValue("tenantId")
                )
            );

            services.AddSingleton<ICosmosDbClientService, CosmosDbClientService>(
                provider => new CosmosDbClientService(
                    connectionString: AppSettingsHelper.GetSettingValue("cosmosDbConnectionString"),
                    databaseName: AppSettingsHelper.GetSettingValue("cosmosDbDatabaseName")
                )
            );

            services.AddSingleton<IQueueClientService, QueueClientService>();
            services.AddSingleton<IConfigService, ConfigService>();
        })
    .Build();

host.Run();