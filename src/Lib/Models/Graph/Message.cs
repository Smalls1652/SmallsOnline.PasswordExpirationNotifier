using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents a message.
/// </summary>
public class Message : IMessage
{
    [System.Text.Json.Serialization.JsonConstructor]
    public Message()
    {}

    public Message(string subject, MessageBody body, Recipient[] toRecipient, FileAttachment[]? attachments)
    {
        Subject = subject;
        Body = body;
        ToRecipient = toRecipient;

        if (attachments is not null)
        {
            Attachments = attachments;
            HasAttachments = true;
        }
        else
        {
            HasAttachments = false;
        }
    }

    /// <inheritdoc />
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("body")]
    public MessageBody Body { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("toRecipients")]
    public Recipient[] ToRecipient { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("ccRecipients")]
    public Recipient[]? CcRecipient { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("attachments")]
    public FileAttachment[]? Attachments { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("hasAttachments")]
    public bool HasAttachments { get; set; }
}