using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for creating new user search configs.
/// </summary>
public partial class NewUserRedirectConfig : ComponentBase
{
    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<NewUserRedirectConfig> _logger { get; set; } = null!;

    /// <summary>
    /// Authentication state provider.
    /// </summary>
    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private UserEmailRedirectConfig? _userEmailRedirect;

    protected override async Task OnInitializedAsync()
    {
        // Get the authentication state.
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, create a new user redirect config.
            _userEmailRedirect = new()
            {
                Id = Guid.NewGuid().ToString(),
                PartitionKey = "user-redirect-config"
            };
        }
        else
        {
            // If the user is not authenticated, log it.
            _logger.LogInformation("User is not authenticated.");
        }
    }
}