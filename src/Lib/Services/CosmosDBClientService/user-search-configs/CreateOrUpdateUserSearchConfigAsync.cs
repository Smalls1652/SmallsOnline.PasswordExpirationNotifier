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

        try
        {
            // Try to create the item.
            await container.UpsertItemAsync(
                item: userSearchConfig,
                partitionKey: new(userSearchConfig.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemAsync(
                item: userSearchConfig,
                id: userSearchConfig.Id,
                partitionKey: new(userSearchConfig.PartitionKey)
            );
        }
    }
}