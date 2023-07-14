using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for listing user search configs.
/// </summary>
public partial class UserEmailRedirectConfigs : ComponentBase
{
    /// <summary>
    /// Dependency injected service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Dependency injected service for interacting with the Queue.
    /// </summary>
    [Inject]
    protected IQueueClientService _queueClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<UserEmailRedirectConfigs> _logger { get; set; } = null!;

    /// <summary>
    /// Authentication state provider.
    /// </summary>
    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private UserEmailRedirectConfig[]? _userEmailRedirects;

    private bool _loading = false;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        // Get the authentication state.
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, get the user redirect configs.
            _logger.LogInformation("Getting user redirect configs...");
            _userEmailRedirects = await _cosmosDbClientService.GetUserRedirectConfigsAsync();
        }
        else
        {
            // If the user is not authenticated, log it.
            _logger.LogInformation("User is not authenticated.");
        }

        _loading = false;
    }

    /// <summary>
    /// Handle the callback from the delete modal to remove a user redirect config.
    /// </summary>
    /// <param name="id">The ID of the <see cref="UserEmailRedirectConfig" />.</param>
    private async Task HandleRemoveUserRedirectConfigAsync(string id)
    {
        _logger.LogWarning("Removing user search config with id: {id}", id);
        await _cosmosDbClientService.RemoveUserRedirectConfigAsync(id);
        _userEmailRedirects = Array.FindAll(_userEmailRedirects!, item => item.Id != id);
    }


}