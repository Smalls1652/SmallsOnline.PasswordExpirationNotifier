using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task<UserEmailRedirectConfig[]?> GetUserRedirectConfigsAsync()
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the total number of configs.
        int totalConfigCount = await GetTotalItemCountAsync("user-redirect-config");

        // If no configs were found, throw an exception.
        if (totalConfigCount == 0)
        {
            return null;
        }

        // Create an array to hold the configs.
        UserEmailRedirectConfig[] configs = new UserEmailRedirectConfig[totalConfigCount];

        QueryDefinition configsQuery = new("SELECT * FROM c");

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: configsQuery,
            requestOptions: new()
            {
                PartitionKey = new("user-redirect-config")
            }
        );

        int i = 0;
        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();
            using StreamReader streamReader = new(response.Content);
            CosmosDbResponse<UserEmailRedirectConfig>? configsResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: streamReader.BaseStream,
                jsonTypeInfo: _jsonSourceGenerationContext.CosmosDbResponseUserEmailRedirectConfig
            );

            for (var n = 0; n < configsResponse!.Documents!.Length; n++)
            {
                configs[i] = configsResponse.Documents[n];
                i++;
            }
        }

        return configs;
    }
}