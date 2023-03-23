using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds info for a file attachment to an email template.
/// </summary>
public class EmailTemplateAttachmentItem : IEmailTemplateAttachmentItem
{
    /// <summary>
    /// A unique ID for the attachment.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The partition key for the attachment.
    /// </summary>
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <summary>
    /// The name of the file.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// A base64 encoded string of the file content.
    /// </summary>
    [JsonPropertyName("fileContents")]
    public string? FileContents { get; set; }

    /// <summary>
    /// Whether the attachment should be displayed inline with the email body.
    /// </summary>
    [JsonPropertyName("isInline")]
    public bool IsInline { get; set; }

    [JsonIgnore]
    public bool IsUploaded { get; set; } = true;
}