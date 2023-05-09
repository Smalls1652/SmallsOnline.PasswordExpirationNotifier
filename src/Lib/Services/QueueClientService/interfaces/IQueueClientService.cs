using Azure.Storage.Queues;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Interface for the queue client service.
/// </summary>
public interface IQueueClientService
{
    /// <summary>
    /// Queue client for the user search queue.
    /// </summary>
    QueueClient UserSearchQueueClient { get; }

    /// <summary>
    /// Queue client for the email queue.
    /// </summary>
    QueueClient EmailQueueClient { get; }
}