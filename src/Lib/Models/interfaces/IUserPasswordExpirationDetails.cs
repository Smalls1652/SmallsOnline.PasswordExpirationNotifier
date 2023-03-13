using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

public interface IUserPasswordExpirationDetails
{
    string Id { get; set; }
    User User { get; set; }
    string UserSearchConfigId { get; set; }
    int MaxPasswordAge { get; set; }
    DateTimeOffset PasswordLastSetDate { get; set; }
    TimeSpan PasswordLifespan { get; }
    DateTimeOffset PasswordExpirationDate { get; }
    TimeSpan PasswordExpiresIn { get; }
    bool PasswordIsExpired { get; }
    bool PasswordIsExpiringSoon { get; }
}