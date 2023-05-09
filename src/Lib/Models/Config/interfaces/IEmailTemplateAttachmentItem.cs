namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Interface for a file attachment to use in an email template.
/// </summary>
public interface IEmailTemplateAttachmentItem
{
    /// <summary>
    /// A unique ID for the attachment.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key for the attachment.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// The name of the file.
    /// </summary>
    string FileName { get; set; }

    /// <summary>
    /// A base64 encoded string of the file content.
    /// </summary>
    string? FileContents { get; set; }

    /// <summary>
    /// Whether the attachment should be displayed inline with the email body.
    /// </summary>
    bool IsInline { get; set; }

    /// <summary>
    /// Whether the attachment has been uploaded to the DB.
    /// </summary>
    bool IsUploaded { get; set; }
}