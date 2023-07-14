using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(EmailAddress))]
[JsonSerializable(typeof(FileAttachment))]
[JsonSerializable(typeof(FileAttachment[]))]
[JsonSerializable(typeof(MailMessage))]
[JsonSerializable(typeof(Message))]
[JsonSerializable(typeof(MessageBody))]
[JsonSerializable(typeof(Recipient))]
[JsonSerializable(typeof(Recipient[]))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(User[]))]
[JsonSerializable(typeof(UserCollection))]
[JsonSerializable(typeof(GraphErrorResponse))]
[JsonSerializable(typeof(GraphError))]
internal partial class GraphJsonContext : JsonSerializerContext
{
}