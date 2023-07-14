using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task<EmailTemplateConfig[]?> GetEmailTemplateConfigsAsync()
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the total number of configs.
        int totalConfigCount = await GetTotalItemCountAsync("email-template-config");

        // If no configs were found, throw an exception.
        if (totalConfigCount == 0)
        {
            return null;
        }

        // Create an array to hold the configs.
        EmailTemplateConfig[] configs = new EmailTemplateConfig[totalConfigCount];

        // Define the query to get the IDs of all configs.
        QueryDefinition configsQuery = new("SELECT c.id, c.partitionKey FROM c");

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: configsQuery,
            requestOptions: new()
            {
                PartitionKey = new("email-template-config")
            }
        );

        int i = 0;
        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();
            using StreamReader streamReader = new(response.Content);
            CosmosDbResponse<EmailTemplateConfig>? configsResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: streamReader.BaseStream,
                jsonTypeInfo: CosmosDbJsonContext.Default.CosmosDbResponseEmailTemplateConfig
            );

            foreach (EmailTemplateConfig config in configsResponse!.Documents!)
            {
                configs[i] = await GetEmailTemplateConfigAsync(config.Id);
                i++;
            }
        }

        return configs;
    }
}