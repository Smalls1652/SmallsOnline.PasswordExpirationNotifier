namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing a recipient item in Microsoft Graph.
/// </summary>
public interface IRecipient
{
    /// <summary>
    /// The email address of the recipient.
    /// </summary>
    EmailAddress EmailAddress { get; set; }
}