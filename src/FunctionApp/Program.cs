using Microsoft.ApplicationInsights;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

IHostBuilder hostBuilder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        if (AppSettingsHelper.GetSettingValue("APPLICATIONINSIGHTS_CONNECTION_STRING") is not null && AppSettingsHelper.GetSettingValue("APPINSIGHTS_INSTRUMENTATIONKEY") is not null)
        {
            builder.AddApplicationInsights();
            builder.AddApplicationInsightsLogger();
        }
    })
    .ConfigureServices(
        services =>
        {
            services.AddSingleton<IGraphClientService, GraphClientService>(
                provider => new GraphClientService(
                    config: new()
                    {
                        ClientId = AppSettingsHelper.GetSettingValue("clientId")!,
                        TenantId = AppSettingsHelper.GetSettingValue("tenantId")!,
                        Credential = new GraphClientCredential(
                            credentialType: GraphClientCredentialType.ClientSecret,
                            clientSecret: AppSettingsHelper.GetSettingValue("clientSecret")!
                        ),
                        ApiScopes = new[]
                        {
                            "https://graph.microsoft.com/.default"
                        }
                    }
                )
            );

            services.AddSingleton<ICosmosDbClientService, CosmosDbClientService>(
                provider => new CosmosDbClientService(
                    connectionString: AppSettingsHelper.GetSettingValue("cosmosDbConnectionString")!,
                    databaseName: AppSettingsHelper.GetSettingValue("cosmosDbDatabaseName")!
                )
            );

            services.AddSingleton<IQueueClientService, QueueClientService>(
                provider => new(
                    connectionString: AppSettingsHelper.GetSettingValue("storageConnectionString")!
                )
            );

            services.AddSingleton<TelemetryClient>();

            services.AddTransient<IConfigService, ConfigService>();
        });

var host = hostBuilder.Build();

await host.RunAsync();