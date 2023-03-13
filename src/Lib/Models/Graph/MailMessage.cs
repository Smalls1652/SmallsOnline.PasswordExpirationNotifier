using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents a message to be sent.
/// </summary>
public class MailMessage : IMailMessage
{
    [JsonConstructor]
    public MailMessage()
    {}

    public MailMessage(Message message, bool saveToSentItems)
    {
        Message = message;
        SaveToSentItems = saveToSentItems;
    }

    /// <inheritdoc />
    [JsonPropertyName("message")]
    public Message Message { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("saveToSentItems")]
    public bool SaveToSentItems { get; set; }
}