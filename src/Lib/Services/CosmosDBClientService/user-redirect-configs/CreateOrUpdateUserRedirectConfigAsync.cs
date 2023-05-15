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

        try
        {
            // Try to create the item.
            await container.UpsertItemAsync(
                item: userEmailRedirectConfig,
                partitionKey: new(userEmailRedirectConfig.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemAsync(
                item: userEmailRedirectConfig,
                id: userEmailRedirectConfig.Id,
                partitionKey: new(userEmailRedirectConfig.PartitionKey)
            );
        }
    }
}