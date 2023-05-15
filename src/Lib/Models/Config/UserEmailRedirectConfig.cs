using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public class UserEmailRedirectConfig : IUserEmailRedirectConfig
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    [JsonPropertyName("userPrincipalName")]
    public string? UserPrincipalName { get; set; }

    [JsonPropertyName("redirectUserPrincipalName")]
    public string? RedirectUserPrincipalName { get; set; }
}