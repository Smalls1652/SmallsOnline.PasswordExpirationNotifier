namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface for a generic Microsoft Graph collection response.
/// </summary>
/// <typeparam name="T">The collection value type.</typeparam>
public interface ICollectionResponse<T>
{
    /// <summary>
    /// The number of items in the collection.
    /// </summary>
    int OdataCount { get; set; }

    /// <summary>
    /// The current OData context.
    /// </summary>
    string? ODataContext { get; set; }

    /// <summary>
    /// The next link for paged OData requests.
    /// </summary>
    string? ODataNextLink { get; set; }

    /// <summary>
    /// A collection of the data returned.
    /// </summary>
    T[]? Value { get; set; }
}