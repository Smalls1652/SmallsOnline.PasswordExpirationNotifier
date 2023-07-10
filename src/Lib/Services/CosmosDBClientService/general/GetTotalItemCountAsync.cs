using Microsoft.Azure.Cosmos;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    private async Task<int> GetTotalItemCountAsync(string partitionKey)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Define the query to get the total number of items.
        QueryDefinition countQuery = new("SELECT VALUE COUNT(1) FROM c");

        // Get the total number of configs.
        FeedResponse<int> countQueryResponse = await container.GetItemQueryIterator<int>(
                queryDefinition: countQuery,
                requestOptions: new()
                {
                    PartitionKey = new(partitionKey),
                    MaxItemCount = 1
                }
            )
            .ReadNextAsync();
        int totalConfigCount = countQueryResponse.FirstOrDefault();

        return totalConfigCount;
    }
}