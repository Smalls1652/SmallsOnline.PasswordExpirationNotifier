namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public interface IUserSearchConfig
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string? ConfigName { get; set; }
    bool ConfigEnabled { get; set; }
    string? ConfigDescription { get; set; }
    DateTimeOffset? ConfigLastModified { get; set; }
    string? DomainName { get; set; }
    string? OUPath { get; set; }
    int MaxPasswordAge { get; set; }
    bool IsEmailIntervalsEnabled { get; set; }
    List<EmailIntervalDay>? EmailIntervalDays { get; set; }
    string? EmailTemplateId { get; set; }
}