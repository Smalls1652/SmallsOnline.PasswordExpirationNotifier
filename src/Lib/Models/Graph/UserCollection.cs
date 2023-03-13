using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// A collection of <see cref="User"/> items returned by Microsoft Graph.
/// </summary>
public class UserCollection : ICollectionResponse<User>
{
    /// <summary>
    /// The number of items in the collection.
    /// </summary>
    [JsonPropertyName("@odata.count")]
    public int OdataCount { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("@odata.context")]
    public string? ODataContext { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("@odata.nextLink")]
    public string? ODataNextLink { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("value")]
    public User[]? Value { get; set; }
}