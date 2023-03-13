namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing a mail message item in Microsoft Graph.
/// </summary>
public interface IMailMessage
{
    Message Message { get; set; }

    /// <summary>
    /// Whether to save the message in the Sent Items folder or not.
    /// </summary>
    bool SaveToSentItems { get; set; }
}