using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task<UserSearchConfig[]?> GetUserSearchConfigsAsync()
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the total number of configs.
        int totalConfigCount = await GetTotalItemCountAsync("user-search-config");

        // If no configs were found, throw an exception.
        if (totalConfigCount == 0)
        {
            return null;
        }

        // Create an array to hold the configs.
        UserSearchConfig[] configs = new UserSearchConfig[totalConfigCount];

        // Define the query to get the IDs of all configs.
        QueryDefinition configsQuery = new("SELECT * FROM c");

        using FeedIterator feedIterator = container.GetItemQueryStreamIterator(
            queryDefinition: configsQuery,
            requestOptions: new()
            {
                PartitionKey = new("user-search-config")
            }
        );

        int i = 0;
        while (feedIterator.HasMoreResults)
        {
            using ResponseMessage response = await feedIterator.ReadNextAsync();
            using StreamReader streamReader = new(response.Content);
            CosmosDbResponse<UserSearchConfig>? configsResponse = await JsonSerializer.DeserializeAsync(
                utf8Json: streamReader.BaseStream,
                jsonTypeInfo: CosmosDbJsonContext.Default.CosmosDbResponseUserSearchConfig
            );

            foreach (UserSearchConfig config in configsResponse!.Documents!)
            {
                configs[i] = config;
                i++;
            }
        }

        return configs;
    }
}