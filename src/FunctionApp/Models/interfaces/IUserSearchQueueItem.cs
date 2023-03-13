namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp.Models;

public interface IUserSearchQueueItem
{
    string UserSearchConfigId { get; set; }
    string DomainName { get; set; }
    string LastNameStartsWith { get; set; }
    string? OUPath { get; set; }
    int MaxPasswordAge { get; set; }
    bool IsEmailIntervalsEnabled { get; set; }
    int[]? EmailIntervalDays { get; set; }
    string EmailTemplateId { get; set; }
}