using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for listing user search configs.
/// </summary>
public partial class UserSearchConfigs : ComponentBase
{
    /// <summary>
    /// Dependency injected service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected IQueueClientService _queueClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<UserSearchConfigs> _logger { get; set; } = null!;

    /// <summary>
    /// Authentication state provider.
    /// </summary>
    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private UserSearchConfig[]? _userSearchConfigs;
    private JsonSourceGenerationContext _jsonSourceGenerationContext = new();

    private bool _loading = false;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        // Get the authentication state.
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, get the user search configs.
            _logger.LogInformation("Getting user search configs...");
            _userSearchConfigs = await _cosmosDbClientService.GetUserSearchConfigsAsync();
        }
        else
        {
            // If the user is not authenticated, log it.
            _logger.LogInformation("User is not authenticated.");
        }

        _loading = false;
    }

    private async Task HandleInvokeUserSearchConfigAsync(string id)
    {
        _logger.LogWarning("Invoking user search config with id: {id}", id);
        UserSearchConfig configItem = await _cosmosDbClientService.GetUserSearchConfigAsync(id);

        Range range = new(65, 90);
        for (int i = range.Start.Value; i <= range.End.Value; i++)
        {
            string lastNameStartsWithChar = Convert.ToChar(i).ToString();
            _logger.LogInformation("Creating queue message for config '{ConfigName} [{ConfigId}]' with last name starting with '{LastNameStartsWithChar}'.", configItem.ConfigName, configItem.Id, lastNameStartsWithChar);

            // Create the queue message.
            await _queueClientService.UserSearchQueueClient.SendMessageAsync(
                messageText: JsonSerializer.Serialize(
                    value: new UserSearchQueueItem()
                    {
                        UserSearchConfigId = configItem.Id,
                        DomainName = configItem.DomainName!,
                        LastNameStartsWith = lastNameStartsWithChar,
                        OUPath = configItem.OUPath,
                        MaxPasswordAge = configItem.MaxPasswordAge,
                        IsEmailIntervalsEnabled = configItem.IsEmailIntervalsEnabled,
                        EmailIntervalDays = configItem.EmailIntervalDays,
                        EmailTemplateId = configItem.EmailTemplateId!
                    },
                    jsonTypeInfo: _jsonSourceGenerationContext.UserSearchQueueItem
                )
            );
        }
    }

    private async Task HandleRemoveUserSearchConfigAsync(string id)
    {
        _logger.LogWarning("Removing user search config with id: {id}", id);
        await _cosmosDbClientService.RemoveUserSearchConfigAsync(id);
        _userSearchConfigs = Array.FindAll(_userSearchConfigs!, item => item.Id != id);
    }


}