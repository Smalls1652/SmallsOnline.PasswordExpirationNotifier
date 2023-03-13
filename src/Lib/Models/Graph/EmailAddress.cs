using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents an email address.
/// </summary>
public class EmailAddress : IEmailAddress
{
    [JsonConstructor]
    public EmailAddress()
    {}

    public EmailAddress(string address, string? name)
    {
        Address = address;
        Name = name;
    }

    /// <inheritdoc />
    [JsonPropertyName("address")]
    public string Address { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}