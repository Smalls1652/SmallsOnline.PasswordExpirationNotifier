namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

public interface IGraphError
{
    string? Code { get; set; }
    string? Message { get; set; }
}