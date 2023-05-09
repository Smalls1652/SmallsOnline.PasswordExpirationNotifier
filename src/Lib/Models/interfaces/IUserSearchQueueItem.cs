using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Interface for a user search to be performed.
/// </summary>
public interface IUserSearchQueueItem
{
    /// <summary>
    /// The ID of the the user search config.
    /// </summary>
    string UserSearchConfigId { get; set; }

    /// <summary>
    /// The domain name to search for users by.
    /// </summary>
    string? DomainName { get; set; }

    /// <summary>
    /// The first character of the users' last name to search by.
    /// </summary>
    string LastNameStartsWith { get; set; }

    /// <summary>
    /// The OU path to search for users by.
    /// </summary>
    string? OUPath { get; set; }

    /// <summary>
    /// The maximum password age in days.
    /// </summary>
    int MaxPasswordAge { get; set; }

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
    string EmailTemplateId { get; set; }
}