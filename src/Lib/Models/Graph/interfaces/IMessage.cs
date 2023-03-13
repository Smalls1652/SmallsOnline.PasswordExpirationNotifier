namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing a mail message item in Microsoft Graph.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// The subject of the message.
    /// </summary>
    string Subject { get; set; }

    /// <summary>
    /// The body of the message.
    /// </summary>
    MessageBody Body { get; set; }

    /// <summary>
    /// Direct 'to' recipients for the message.
    /// </summary>
    Recipient[] ToRecipient { get; set; }

    /// <summary>
    /// CC'd recipients for the message.
    /// </summary>
    Recipient[]? CcRecipient { get; set; }

    /// <summary>
    /// File attachments for the message.
    /// </summary>
    FileAttachment[]? Attachments { get; set; }

    /// <summary>
    /// Whether the message has attachments or not.
    /// </summary>
    bool HasAttachments { get; set; }
}