using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(UserSearchQueueItem))]
[JsonSerializable(typeof(UserSearchQueueItem[]))]
[JsonSerializable(typeof(UserPasswordExpirationDetails))]
[JsonSerializable(typeof(UserPasswordExpirationDetails[]))]
internal partial class QueueJsonContext : JsonSerializerContext
{
}