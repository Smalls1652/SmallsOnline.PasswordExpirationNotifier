using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Users;

/// <summary>
/// Component for displaying user data.
/// </summary>
public partial class UserDataComponent : ComponentBase
{
    /// <summary>
    /// The user data, from the Graph API, to display.
    /// </summary>
    [Parameter]
    public User UserData { get; set; } = null!;

    /// <summary>
    /// The user search configuration for parsing password expiration details.
    /// </summary>
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