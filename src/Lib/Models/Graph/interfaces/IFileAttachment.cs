namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing a file attachment item in Microsoft Graph.
/// </summary>
public interface IFileAttachment
{
    /// <summary>
    /// The MIME type of the attachment.
    /// </summary>
    string DataType { get; set; }

    /// <summary>
    /// The file name of the attachment.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// The base64 encoded contents of the attachment.
    /// </summary>
    string ContentBytes { get; set; }

    /// <summary>
    /// Whether the attachment is an inline attachment or not.
    /// </summary>
    bool IsInline { get; set; }
}