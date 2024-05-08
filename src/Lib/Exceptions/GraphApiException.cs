using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib;

public class GraphApiException : Exception
{
    public GraphApiException(string message) : base(message) { }

    public GraphApiException(string message, GraphErrorResponse? errorResponse) : base(message)
    {
        ErrorResponse = errorResponse;
    }

    public GraphApiException(string message, Exception innerException) : base(message, innerException) { }

    public GraphApiException(string message, GraphErrorResponse? errorResponse, Exception innerException) : base(message, innerException)
    {
        ErrorResponse = errorResponse;
    }

    public GraphErrorResponse? ErrorResponse { get; }
}