using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for editing an user redirect config.
/// </summary>
public partial class EditUserRedirectConfig : ComponentBase
{
    /// <summary>
    /// Dependency injected service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<EditUserRedirectConfig> _logger { get; set; } = null!;

    /// <summary>
    /// Authentication state provider.
    /// </summary>
    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    /// <summary>
    /// The ID of the user search config to edit.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = null!;

    private UserEmailRedirect? _userEmailRedirect;

    protected override async Task OnInitializedAsync()
    {
        // Get the authentication state.
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, get the user redirect config.
            _logger.LogInformation("Getting user redirect config...");
            _userEmailRedirect = await _cosmosDbClientService.GetUserRedirectConfigAsync(Id);
        }
        else
        {
            // If the user is not authenticated, log it.
            _logger.LogInformation("User is not authenticated.");
        }
    }
}