using System.Text;
using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds info for an email template.
/// </summary>
public class EmailTemplateConfig : IEmailTemplateConfig
{
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("templateName")]
    public string? TemplateName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("templateDescription")]
    public string? TemplateDescription { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("templateLastModified")]
    public DateTimeOffset? TemplateLastModified { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("templateSendAsUser")]
    public string? TemplateSendAsUser { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? TemplateHtml
    {
        get => TemplateHtmlBase64 is not null ? Encoding.UTF8.GetString(Convert.FromBase64String(TemplateHtmlBase64)) : null;
        set => TemplateHtmlBase64 = value is not null ? Convert.ToBase64String(Encoding.UTF8.GetBytes(value)) : null;
    }

    /// <inheritdoc />
    [JsonPropertyName("templateHtmlBase64")]
    public string? TemplateHtmlBase64 { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public EmailTemplateAttachmentItem[]? IncludedAttachments { get; set; }


    [JsonPropertyName("includedAttachmentIds")]
    public string[]? IncludedAttachmentIds { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("isCustomTemplate")]
    public bool IsCustomTemplate { get; set; }
}