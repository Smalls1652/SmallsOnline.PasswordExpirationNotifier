namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

public interface IGraphErrorResponse
{
    GraphError? Error { get; set; }
}