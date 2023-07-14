using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(UserSearchConfig))]
[JsonSerializable(typeof(UserSearchConfig[]))]
[JsonSerializable(typeof(UserPasswordExpirationDetails))]
[JsonSerializable(typeof(UserPasswordExpirationDetails[]))]
[JsonSerializable(typeof(EmailTemplateConfig))]
[JsonSerializable(typeof(EmailTemplateConfig[]))]
[JsonSerializable(typeof(EmailTemplateAttachmentItem))]
[JsonSerializable(typeof(EmailTemplateAttachmentItem[]))]
[JsonSerializable(typeof(EmailIntervalDay))]
[JsonSerializable(typeof(List<EmailIntervalDay>))]
[JsonSerializable(typeof(UserEmailRedirectConfig))]
[JsonSerializable(typeof(UserEmailRedirectConfig[]))]
[JsonSerializable(typeof(CosmosDbResponse<UserEmailRedirectConfig>))]
[JsonSerializable(typeof(CosmosDbResponse<UserSearchConfig>))]
[JsonSerializable(typeof(CosmosDbResponse<EmailTemplateConfig>))]
[JsonSerializable(typeof(CosmosDbResponse<EmailTemplateAttachmentItem>))]
internal partial class CosmosDbJsonContext : JsonSerializerContext
{
}