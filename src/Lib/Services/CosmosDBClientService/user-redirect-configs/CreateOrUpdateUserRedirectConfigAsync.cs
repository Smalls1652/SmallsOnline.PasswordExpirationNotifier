using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task CreateOrUpdateUserRedirectConfigAsync(UserEmailRedirectConfig userEmailRedirectConfig)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        using MemoryStream streamPayload = new();

        await JsonSerializer.SerializeAsync(
            utf8Json: streamPayload,
            value: userEmailRedirectConfig,
            jsonTypeInfo: CosmosDbJsonContext.Default.UserEmailRedirectConfig
        );

        try
        {
            // Try to create the item.
            await container.UpsertItemStreamAsync(
                streamPayload: streamPayload,
                partitionKey: new(userEmailRedirectConfig.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemStreamAsync(
                streamPayload: streamPayload,
                id: userEmailRedirectConfig.Id,
                partitionKey: new(userEmailRedirectConfig.PartitionKey)
            );
        }
    }
}