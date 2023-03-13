using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents a recipient for a message.
/// </summary>
public class Recipient : IRecipient
{
    [JsonConstructor]
    public Recipient()
    {}

    public Recipient(string emailAddress, string? name)
    {
        EmailAddress = new EmailAddress(emailAddress, name);
    }

    /// <inheritdoc />
    [JsonPropertyName("emailAddress")]
    public EmailAddress EmailAddress { get; set; } = null!;
}