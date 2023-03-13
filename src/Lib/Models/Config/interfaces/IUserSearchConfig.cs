namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public interface IUserSearchConfig
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string ConfigName { get; set; }
    string DomainName { get; set; }
    string? OUPath { get; set; }
    int MaxPasswordAge { get; set; }
    bool IsEmailIntervalsEnabled { get; set; }
    int[]? EmailIntervalDays { get; set; }
    string EmailTemplateId { get; set; }
}