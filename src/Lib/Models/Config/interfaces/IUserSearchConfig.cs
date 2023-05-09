namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Interface for user search configs.
/// </summary>
public interface IUserSearchConfig
{
    /// <summary>
    /// The unique ID of the config.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key for the config.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// The name of the config.
    /// </summary>
    string? ConfigName { get; set; }

    /// <summary>
    /// Whether the config is enabled or not.
    /// </summary>
    bool ConfigEnabled { get; set; }

    /// <summary>
    /// The description of the config.
    /// </summary>
    string? ConfigDescription { get; set; }

    /// <summary>
    /// The last time the config was modified.
    /// </summary>
    DateTimeOffset? ConfigLastModified { get; set; }

    /// <summary>
    /// The domain name to search for users by.
    /// </summary>
    string? DomainName { get; set; }

    /// <summary>
    /// The OU path to search for users by.
    /// </summary>
    string? OUPath { get; set; }

    /// <summary>
    /// The maximum password age in days.
    /// </summary>
    int MaxPasswordAge { get; set; }

    /// <summary>
    /// Whether or not to ignore the password age during searches.
    /// </summary>
    bool IgnorePasswordAge { get; set; }

    /// <summary>
    /// Whether to send emails in intervals, rather than every day.
    /// </summary>
    bool IsEmailIntervalsEnabled { get; set; }

    /// <summary>
    /// The days to send emails on when <see cref="IsEmailIntervalsEnabled"/> is true.
    /// </summary>
    List<EmailIntervalDay>? EmailIntervalDays { get; set; }

    /// <summary>
    /// The ID of the email template to use.
    /// </summary>
    string? EmailTemplateId { get; set; }

    /// <summary>
    /// Whether or not emails should be sent.
    /// </summary>
    bool DoNotSendEmails { get; set; }

    /// <summary>
    /// The default timezone the emails should use in the email.
    /// </summary>
    string? DefaultTimeZone { get; set; }
}