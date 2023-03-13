using Azure.Storage.Queues;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;

public interface IQueueClientService
{
    QueueClient UserSearchQueueClient { get; }
    QueueClient EmailQueueClient { get; }
}