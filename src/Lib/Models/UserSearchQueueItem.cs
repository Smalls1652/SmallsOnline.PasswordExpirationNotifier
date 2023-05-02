using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Holds information about a user search to be performed.
/// </summary>
public class UserSearchQueueItem : IUserSearchQueueItem
{
    /// <summary>
    /// The ID of the the user search config.
    /// </summary>
    [JsonPropertyName("userSearchConfigId")]
    public string UserSearchConfigId { get; set; } = null!;

    /// <summary>
    /// The domain name to search for users by.
    /// </summary>
    [JsonPropertyName("domainName")]
    public string? DomainName { get; set; }

    /// <summary>
    /// The first character of the users' last name to search by.
    /// </summary>
    [JsonPropertyName("lastNameStartsWith")]
    public string LastNameStartsWith { get; set; } = null!;

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
    public List<EmailIntervalDay>? EmailIntervalDays { get; set; }

    /// <summary>
    /// The ID of the email template to use.
    /// </summary>
    [JsonPropertyName("emailTemplateId")]
    public string EmailTemplateId { get; set; } = null!;
}