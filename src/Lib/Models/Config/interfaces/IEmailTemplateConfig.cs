namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public interface IEmailTemplateConfig
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string? TemplateName { get; set; }
    string? TemplateDescription { get; set; }
    DateTimeOffset? TemplateLastModified { get; set; }
    string? TemplateSendAsUser { get; set; }
    string? TemplateHtml { get; set; }
    string? TemplateHtmlBase64 { get; set; }
    EmailTemplateAttachmentItem[]? IncludedAttachments { get; set; }
    string[]? IncludedAttachmentIds { get; set; }
    bool IsCustomTemplate { get; set; }
}