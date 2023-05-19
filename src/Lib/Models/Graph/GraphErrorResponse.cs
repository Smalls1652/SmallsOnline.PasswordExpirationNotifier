using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

public class GraphErrorResponse : IGraphErrorResponse
{
    [JsonPropertyName("error")]
    public GraphError? Error { get; set; }
}