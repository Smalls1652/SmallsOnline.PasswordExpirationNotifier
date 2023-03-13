using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds info for a user search config.
/// </summary>
public class UserSearchConfig : IUserSearchConfig
{
    /// <summary>
    /// The unique ID of the config.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The partition key for the config.
    /// </summary>
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <summary>
    /// The name of the config.
    /// </summary>
    [JsonPropertyName("configName")]
    public string ConfigName { get; set; } = null!;

    /// <summary>
    /// The domain name to search for users by.
    /// </summary>
    [JsonPropertyName("domainName")]
    public string DomainName { get; set; } = null!;

    /// <summary>
    /// The OU path to search for users by.
    /// </summary>
    [JsonPropertyName("ouPath")]
    public string? OUPath { get; set; }

    /// <summary>
    /// The maximum password age in days.
    /// </summary>
    [JsonPropertyName("maxPasswordAge")]
    public int MaxPasswordAge { get; set; }

    /// <summary>
    /// Whether to send emails in intervals, rather than every day.
    /// </summary>
    [JsonPropertyName("isEmailIntervalsEnabled")]
    public bool IsEmailIntervalsEnabled { get; set; }

    /// <summary>
    /// The days to send emails on when <see cref="IsEmailIntervalsEnabled"/> is true.
    /// </summary>
    [JsonPropertyName("emailIntervalDays")]
    public int[]? EmailIntervalDays { get; set; }

    /// <summary>
    /// The ID of the email template to use.
    /// </summary>
    [JsonPropertyName("emailTemplateId")]
    public string EmailTemplateId { get; set; } = null!;
}