using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for manually sending an email to a user.
/// </summary>
public partial class ManuallySendEmailToUser : ComponentBase
{
    /// <summary>
    /// Dependency injected service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Dependency injected service for interacting with the Microsoft Graph API.
    /// </summary>
    [Inject]
    protected IGraphClientService _graphClientService { get; set; } = null!;

    /// <summary>
    /// Dependency injected service for interacting with the Queue.
    /// </summary>
    [Inject]
    protected IQueueClientService _queueClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<ManuallySendEmailToUser> _logger { get; set; } = null!;

    /// <summary>
    /// Dependency injected instance of the configuration.
    /// </summary>
    [Inject]
    protected IConfiguration _configuration { get; set; } = null!;

    private ManualSendEmailItem _manualSendEmailItem = new();

    private UserSearchConfig[]? _userSearchConfigs;

    private bool _isLoaded;

    protected override async Task OnInitializedAsync()
    {
        _isLoaded = false;
        _logger.LogInformation("Initializing manually send email to user page...");
        _userSearchConfigs = await _cosmosDbClientService.GetUserSearchConfigsAsync();
        _isLoaded = true;
    }

    /// <summary>
    /// Handles the form submit event.
    /// </summary>
    private async Task HandleFormSubmitAsync()
    {

        _logger.LogInformation("User principal name: {UserPrincipalName}", _manualSendEmailItem.UserPrincipalName);
        _logger.LogInformation("Search config ID: {SearchConfigId}", _manualSendEmailItem.UserSearchConfigId);

        // Get the user search config.
        UserSearchConfig userSearchConfig = _userSearchConfigs!.First(x => x.Id == _manualSendEmailItem.UserSearchConfigId);

        // Get the details for the user from the Microsoft Graph API.
        _logger.LogInformation("Getting user details for: {UserPrincipalName}...", _manualSendEmailItem.UserPrincipalName);
        User user = await _graphClientService.GetUserAsync(_manualSendEmailItem.UserPrincipalName!);

        // Create the password expiration details object and then send it to the queue.
        UserPasswordExpirationDetails passwordExpirationDetails = new(
            user: user,
            searchConfig: userSearchConfig
        );

        await _queueClientService.EmailQueueClient.SendMessageAsync(
            JsonSerializer.Serialize(passwordExpirationDetails)
        );

        // Clear the form.
        _manualSendEmailItem = new();
    }
}