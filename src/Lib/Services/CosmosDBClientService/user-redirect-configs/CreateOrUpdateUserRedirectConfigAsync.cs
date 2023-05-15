using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task CreateOrUpdateUserRedirectConfigAsync(UserEmailRedirect userEmailRedirect)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        try
        {
            // Try to create the item.
            await container.UpsertItemAsync(
                item: userEmailRedirect,
                partitionKey: new(userEmailRedirect.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemAsync(
                item: userEmailRedirect,
                id: userEmailRedirect.Id,
                partitionKey: new(userEmailRedirect.PartitionKey)
            );
        }
    }
}