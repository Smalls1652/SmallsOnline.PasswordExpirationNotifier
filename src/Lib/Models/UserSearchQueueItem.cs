using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Holds information about a user search to be performed.
/// </summary>
public class UserSearchQueueItem : IUserSearchQueueItem
{
    /// <inheritdoc />
    [JsonPropertyName("userSearchConfigId")]
    public string UserSearchConfigId { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("domainName")]
    public string? DomainName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("lastNameStartsWith")]
    public string LastNameStartsWith { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("ouPath")]
    public string? OUPath { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("maxPasswordAge")]
    public int MaxPasswordAge { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("isEmailIntervalsEnabled")]
    public bool IsEmailIntervalsEnabled { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("emailIntervalDays")]
    public List<EmailIntervalDay>? EmailIntervalDays { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("emailTemplateId")]
    public string EmailTemplateId { get; set; } = null!;
}