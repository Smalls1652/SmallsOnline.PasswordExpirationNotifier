using System.Text;
using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds info for an email template.
/// </summary>
public class EmailTemplateConfig : IEmailTemplateConfig
{
    /// <summary>
    /// A unique ID for the template.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The partition key for the template.
    /// </summary>
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <summary>
    /// A name for the template.
    /// </summary>
    [JsonPropertyName("templateName")]
    public string? TemplateName { get; set; }

    [JsonPropertyName("templateDescription")]
    public string? TemplateDescription { get; set; }

    [JsonPropertyName("templateLastModified")]
    public DateTimeOffset? TemplateLastModified { get; set; }
    
    /// <summary>
    /// The user to send the email template as.
    /// </summary>
    [JsonPropertyName("templateSendAsUser")]
    public string? TemplateSendAsUser { get; set; }

    /// <summary>
    /// The template HTML decoded from <see cref="TemplateHtmlBase64"/>.
    /// </summary>
    [JsonIgnore]
    public string? TemplateHtml
    {
        get => TemplateHtmlBase64 is not null ? Encoding.UTF8.GetString(Convert.FromBase64String(TemplateHtmlBase64)) : null;
        set => TemplateHtmlBase64 = value is not null ? Convert.ToBase64String(Encoding.UTF8.GetBytes(value)) : null;
    }

    /// <summary>
    /// The base64 encoded template HTML.
    /// </summary>
    [JsonPropertyName("templateHtmlBase64")]
    public string? TemplateHtmlBase64 { get; set; }

    /// <summary>
    /// Attachments to include with the email.
    /// </summary>
    [JsonIgnore]
    public EmailTemplateAttachmentItem[]? IncludedAttachments { get; set; }

    /// <summary>
    /// The IDs of the attachments to include with the email.
    /// </summary>
    [JsonPropertyName("includedAttachmentIds")]
    public string[]? IncludedAttachmentIds { get; set; }

    /// <summary>
    /// Whether the template is a custom template or not.
    /// </summary>
    [JsonPropertyName("isCustomTemplate")]
    public bool IsCustomTemplate { get; set; }
}