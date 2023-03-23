﻿using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    public async Task RemoveUserSearchConfigAsync(string id)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        await container.DeleteItemAsync<UserSearchConfig>(
            id: id,
            partitionKey: new("user-search-config")
        );
    }
}