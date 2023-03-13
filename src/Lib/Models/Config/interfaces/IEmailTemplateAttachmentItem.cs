namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public interface IEmailTemplateAttachmentItem
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string FileName { get; set; }
    string FileContents { get; set; }
    bool IsInline { get; set; }
}