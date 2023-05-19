using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Users;

public partial class UserDataComponent : ComponentBase
{
    [Parameter]
    public User UserData { get; set; } = null!;

    [Parameter]
    public UserSearchConfig UserSearchConfig { get; set; } = null!;

    private bool _isLoaded;
    private UserPasswordExpirationDetails? _userPasswordExpirationDetails;

    protected override void OnParametersSet()
    {
        _isLoaded = false;

        _userPasswordExpirationDetails = new(
            user: UserData,
            searchConfig: UserSearchConfig
        );

        _isLoaded = true;
    }
}