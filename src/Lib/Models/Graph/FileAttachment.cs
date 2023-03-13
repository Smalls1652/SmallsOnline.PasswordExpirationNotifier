using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents a file attachment to be used in a message.
/// </summary>
public class FileAttachment : IFileAttachment
{
    [JsonConstructor]
    public FileAttachment()
    {}

    public FileAttachment(string fileName, string fileContentBytesBase64, bool isInline)
    {
        DataType = "#microsoft.graph.fileAttachment";
        Name = fileName;
        ContentBytes = fileContentBytesBase64;
        IsInline = isInline;
    }

    public FileAttachment(string fileName, byte[] fileContentBytes, bool isInline)
    {
        DataType = "#microsoft.graph.fileAttachment";
        Name = fileName;
        ContentBytes = Convert.ToBase64String(fileContentBytes);
        IsInline = isInline;
    }

    /// <inheritdoc />
    [JsonPropertyName("@odata.type")]
    public string DataType { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("contentBytes")]
    public string ContentBytes { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("isInline")]
    public bool IsInline { get; set; }
}