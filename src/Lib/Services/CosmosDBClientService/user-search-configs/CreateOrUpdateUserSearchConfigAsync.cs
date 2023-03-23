using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    public async Task CreateOrUpdateUserSearchConfigAsync(UserSearchConfig userSearchConfig)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        try
        {
            await container.UpsertItemAsync(
                item: userSearchConfig,
                partitionKey: new(userSearchConfig.PartitionKey)
            );
        }
        catch
        {
            await container.ReplaceItemAsync(
                item: userSearchConfig,
                id: userSearchConfig.Id,
                partitionKey: new(userSearchConfig.PartitionKey)
            );
        }
    }
}