using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents the body of a message.
/// </summary>
public class MessageBody : IMessageBody
{
    [JsonConstructor]
    public MessageBody()
    {}

    public MessageBody(string content, string contentType)
    {
        Content = content;
        ContentType = contentType;
    }

    /// <inheritdoc />
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("contentType")]
    public string ContentType { get; set; } = null!;
}