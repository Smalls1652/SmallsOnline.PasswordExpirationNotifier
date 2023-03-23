using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

public class ManualSendEmailItem : IManualSendEmailItem
{
    [JsonPropertyName("userPrincipalName")]
    public string? UserPrincipalName { get; set; }

    [JsonPropertyName("userSearchConfigId")]
    public string? UserSearchConfigId { get; set; }
}