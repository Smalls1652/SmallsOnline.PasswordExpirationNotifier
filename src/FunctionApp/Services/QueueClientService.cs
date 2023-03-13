using Azure.Storage.Queues;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;

/// <summary>
/// Service for interacting with Azure Storage queues.
/// </summary>
public class QueueClientService : IQueueClientService
{
    public QueueClientService()
    {
        // Create the queue client for the user search queue.
        UserSearchQueueClient = new(
            connectionString: AppSettingsHelper.GetSettingValue("storageConnectionString"),
            queueName: "user-search-queue",
            options: new()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            }
        );
        UserSearchQueueClient.CreateIfNotExists();

        // Create the queue client for the email queue.
        EmailQueueClient = new(
            connectionString: AppSettingsHelper.GetSettingValue("storageConnectionString"),
            queueName: "email-queue",
            options: new()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            }
        );
        EmailQueueClient.CreateIfNotExists();
    }

    /// <summary>
    /// Queue client for the user search queue.
    /// </summary>
    public QueueClient UserSearchQueueClient { get; }

    /// <summary>
    /// Queue client for the email queue.
    /// </summary>
    public QueueClient EmailQueueClient { get; }
}