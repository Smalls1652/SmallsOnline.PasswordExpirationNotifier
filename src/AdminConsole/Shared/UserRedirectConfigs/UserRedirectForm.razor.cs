using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.UserRedirectConfigs;

/// <summary>
/// Component for editing an user email redirect config.
/// </summary>
public partial class UserRedirectForm : ComponentBase
{
    /// <summary>
    /// Dependency injected Cosmos DB client service.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the component.
    /// </summary>
    [Inject]
    protected ILogger<UserRedirectForm> _logger { get; set; } = null!;

    /// <summary>
    /// Navigation manager for the component to handle navigation.
    /// </summary>
    [Inject]
    protected NavigationManager _navigationManager { get; set; } = null!;

    /// <summary>
    /// The redirect config to edit.
    /// </summary>
    [Parameter]
    public UserEmailRedirect UserEmailRedirect { get; set; } = null!;

    /// <summary>
    /// Method for handling the form submission.
    /// </summary>
    private async Task HandleFormSubmitAsync()
    {
        _logger.LogInformation("Updating redirect config...");

        // Update the redirect config in the database.
        await _cosmosDbClientService.CreateOrUpdateUserRedirectConfigAsync(UserEmailRedirect);

        // Navigate to the user redirects page.
        _navigationManager.NavigateTo("/user-redirect-configs");
    }
}