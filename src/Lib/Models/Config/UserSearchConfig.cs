using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds info for a user search config.
/// </summary>
public class UserSearchConfig : IUserSearchConfig
{
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("configName")]
    public string? ConfigName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("configEnabled")]
    public bool ConfigEnabled { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("configDescription")]
    public string? ConfigDescription { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("configLastModified")]
    public DateTimeOffset? ConfigLastModified { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("domainName")]
    public string? DomainName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("ouPath")]
    public string? OUPath { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("maxPasswordAge")]
    public int MaxPasswordAge { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("ignorePasswordAge")]
    public bool IgnorePasswordAge { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("isEmailIntervalsEnabled")]
    public bool IsEmailIntervalsEnabled { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("emailIntervalDays")]
    public List<EmailIntervalDay>? EmailIntervalDays { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("emailTemplateId")]
    public string? EmailTemplateId { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("doNotSendEmails")]
    public bool DoNotSendEmails { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("defaultTimeZone")]
    public string? DefaultTimeZone { get; set; }
}