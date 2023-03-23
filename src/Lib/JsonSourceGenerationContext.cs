using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Json;

namespace SmallsOnline.PasswordExpirationNotifier.Lib;

[JsonSourceGenerationOptions(
    WriteIndented = true,
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
[JsonSerializable(typeof(UserSearchConfig))]
[JsonSerializable(typeof(UserSearchConfig[]))]
[JsonSerializable(typeof(UserPasswordExpirationDetails))]
[JsonSerializable(typeof(UserPasswordExpirationDetails[]))]
[JsonSerializable(typeof(EmailTemplateConfig))]
[JsonSerializable(typeof(EmailTemplateConfig[]))]
[JsonSerializable(typeof(EmailTemplateAttachmentItem))]
[JsonSerializable(typeof(EmailTemplateAttachmentItem[]))]
[JsonSerializable(typeof(CosmosResponse))]
[JsonSerializable(typeof(EmailIntervalDay))]
[JsonSerializable(typeof(List<EmailIntervalDay>))]
internal partial class JsonSourceGenerationContext : JsonSerializerContext
{
}