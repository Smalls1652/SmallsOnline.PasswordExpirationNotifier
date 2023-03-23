using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class ManuallySendEmailToUser : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected IGraphClientService _graphClientService { get; set; } = null!;

    [Inject]
    protected IQueueClientService _queueClientService { get; set; } = null!;

    [Inject]
    protected ILogger<ManuallySendEmailToUser> _logger { get; set; } = null!;

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

    private async Task HandleFormSubmitAsync()
    {

        _logger.LogInformation("User principal name: {UserPrincipalName}", _manualSendEmailItem.UserPrincipalName);
        _logger.LogInformation("Search config ID: {SearchConfigId}", _manualSendEmailItem.UserSearchConfigId);

        UserSearchConfig userSearchConfig = _userSearchConfigs!.First(x => x.Id == _manualSendEmailItem.UserSearchConfigId);

        _logger.LogInformation("Getting user details for: {UserPrincipalName}...", _manualSendEmailItem.UserPrincipalName);
        User user = await _graphClientService.GetUserAsync(_manualSendEmailItem.UserPrincipalName!);

        UserPasswordExpirationDetails passwordExpirationDetails = new(
            user: user,
            searchConfig: userSearchConfig
        );

        await _queueClientService.EmailQueueClient.SendMessageAsync(
            JsonSerializer.Serialize(passwordExpirationDetails)
        );

        _manualSendEmailItem = new();
    }
}