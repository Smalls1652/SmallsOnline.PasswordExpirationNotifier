using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <summary>
    /// Remove a user search config from the Cosmos DB database.
    /// </summary>
    /// <param name="id">The ID of the user search config.</param>
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