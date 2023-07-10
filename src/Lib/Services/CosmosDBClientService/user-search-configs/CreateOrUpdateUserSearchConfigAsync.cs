using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task CreateOrUpdateUserSearchConfigAsync(UserSearchConfig userSearchConfig)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        using MemoryStream streamPayload = new();
        await JsonSerializer.SerializeAsync(
            utf8Json: streamPayload,
            value: userSearchConfig,
            jsonTypeInfo: _jsonSourceGenerationContext.UserSearchConfig
        );

        try
        {
            // Try to create the item.
            await container.UpsertItemStreamAsync(
                streamPayload: streamPayload,
                partitionKey: new(userSearchConfig.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemStreamAsync(
                streamPayload: streamPayload,
                id: userSearchConfig.Id,
                partitionKey: new(userSearchConfig.PartitionKey)
            );
        }
    }
}