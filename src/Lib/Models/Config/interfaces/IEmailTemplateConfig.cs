namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Interface for an email template config.
/// </summary>
public interface IEmailTemplateConfig
{
    /// <summary>
    /// A unique ID for the template.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key for the template.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// A name for the template.
    /// </summary>
    string? TemplateName { get; set; }

    /// <summary>
    /// A description for the template.
    /// </summary>
    string? TemplateDescription { get; set; }

    /// <summary>
    /// The last time the template was modified.
    /// </summary>
    DateTimeOffset? TemplateLastModified { get; set; }

    /// <summary>
    /// The user to send the email template as.
    /// </summary>
    string? TemplateSendAsUser { get; set; }

    /// <summary>
    /// The template HTML decoded from <see cref="TemplateHtmlBase64"/>.
    /// </summary>
    string? TemplateHtml { get; set; }

    /// <summary>
    /// The base64 encoded template HTML.
    /// </summary>
    string? TemplateHtmlBase64 { get; set; }

    /// <summary>
    /// Attachments to include with the email.
    /// </summary>
    EmailTemplateAttachmentItem[]? IncludedAttachments { get; set; }

    /// <summary>
    /// The IDs of the attachments to include with the email.
    /// </summary>
    string[]? IncludedAttachmentIds { get; set; }

    /// <summary>
    /// Whether the template is a custom template or not.
    /// </summary>
    bool IsCustomTemplate { get; set; }
}