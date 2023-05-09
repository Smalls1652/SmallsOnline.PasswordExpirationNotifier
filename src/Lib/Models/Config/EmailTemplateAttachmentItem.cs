using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds info for a file attachment to an email template.
/// </summary>
public class EmailTemplateAttachmentItem : IEmailTemplateAttachmentItem
{
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("fileContents")]
    public string? FileContents { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("isInline")]
    public bool IsInline { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsUploaded { get; set; } = true;
}