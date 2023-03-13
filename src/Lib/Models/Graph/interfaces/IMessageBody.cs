namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing a message body item in Microsoft Graph.
/// </summary>
public interface IMessageBody
{
    /// <summary>
    /// The contents of the message.
    /// </summary>
    string Content { get; set; }

    /// <summary>
    /// The type of the message body content. Possible values are text and html.
    /// </summary>
    string ContentType { get; set; }
}