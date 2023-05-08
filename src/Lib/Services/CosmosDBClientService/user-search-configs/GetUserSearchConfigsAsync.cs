﻿using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <summary>
    /// Get all user search configs from Cosmos DB.
    /// </summary>
    /// <returns>A collection of <see cref="UserSearchConfig"/> items stored in the database.</returns>
    /// <exception cref="NullReferenceException">No configs were found in the database.</exception>
    public async Task<UserSearchConfig[]?> GetUserSearchConfigsAsync()
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Define the query to get the total number of configs.
        QueryDefinition countQuery = new("SELECT VALUE COUNT(1) FROM c WHERE c.partitionKey = 'user-search-config'");

        // Get the total number of configs.
        int totalConfigCount = 0;
        FeedResponse<int> countQueryResponse = await container.GetItemQueryIterator<int>(
                queryDefinition: countQuery,
                requestOptions: new()
                {
                    MaxItemCount = 1
                }
            )
            .ReadNextAsync();
        totalConfigCount = countQueryResponse.FirstOrDefault();

        // If no configs were found, throw an exception.
        if (totalConfigCount == 0)
        {
            return null;
        }

        // Create an array to hold the configs.
        UserSearchConfig[] configs = new UserSearchConfig[totalConfigCount];

        // Define the query to get the IDs of all configs.
        QueryDefinition configIdsQuery = new("SELECT VALUE c.id FROM c WHERE c.partitionKey = 'user-search-config'");

        // Get the IDs of all configs and get each config.
        // Note: We're using a FeedIterator<string> here because we're only getting the IDs of the configs,
        // which is then passed to GetEmailTemplateConfigAsync to get the full config. This is done to
        // utilize source generated JSON deserialization when getting the data from Cosmos DB, since
        // the Cosmos DB SDK doesn't support source generated JSON deserialization yet.
        using FeedIterator<string> configsIterator = container.GetItemQueryIterator<string>(configIdsQuery);
        while (configsIterator.HasMoreResults)
        {
            int i = 0;
            foreach (var item in await configsIterator.ReadNextAsync())
            {
                configs[i] = await GetUserSearchConfigAsync(item);
                i++;
            }
        }
        return configs;
    }
}