using Azure.Storage.Queues;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Service for interacting with Azure Storage queues.
/// </summary>
public class QueueClientService : IQueueClientService
{
    public QueueClientService(string connectionString)
    {
        // Create the queue client for the user search queue.
        UserSearchQueueClient = new(
            connectionString: connectionString,
            queueName: "user-search-queue",
            options: new()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            }
        );
        UserSearchQueueClient.CreateIfNotExists();

        // Create the queue client for the email queue.
        EmailQueueClient = new(
            connectionString: connectionString,
            queueName: "email-queue",
            options: new()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            }
        );
        EmailQueueClient.CreateIfNotExists();
    }

    /// <inheritdoc />
    public QueueClient UserSearchQueueClient { get; }

    /// <inheritdoc />
    public QueueClient EmailQueueClient { get; }
}