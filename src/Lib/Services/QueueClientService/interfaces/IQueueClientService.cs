using Azure.Storage.Queues;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public interface IQueueClientService
{
    QueueClient UserSearchQueueClient { get; }
    QueueClient EmailQueueClient { get; }
}