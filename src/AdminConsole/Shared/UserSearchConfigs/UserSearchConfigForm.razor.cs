using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.UserSearchConfigs;

public partial class UserSearchConfigForm : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected ILogger<UserSearchConfigForm> _logger { get; set; } = null!;

    [Inject]
    protected NavigationManager _navigationManager { get; set; } = null!;

    [Parameter]
    public UserSearchConfig UserSearchConfig { get; set; } = null!;

    private List<int>? _emailIntervalDays;

    private EmailTemplateConfig[]? _emailTemplateConfigs;

    protected override async Task OnInitializedAsync()
    {
        if (UserSearchConfig.EmailIntervalDays is not null)
        {
            //_emailIntervalDays = new(UserSearchConfig.EmailIntervalDays);
        }
        else
        {
            _emailIntervalDays = new();
        }

        _emailTemplateConfigs = await _cosmosDbClientService.GetEmailTemplateConfigsAsync();
    }

    private async Task HandleFormSubmitAsync()
    {
        _logger.LogInformation("Updating template...");

        UserSearchConfig.ConfigLastModified = DateTimeOffset.Now;
        //UserSearchConfig.EmailIntervalDays = _emailIntervalDays;

        await _cosmosDbClientService.CreateOrUpdateUserSearchConfigAsync(UserSearchConfig);

        _navigationManager.NavigateTo("/user-search-configs");
    }

    private void HandleAddEmailIntervalDay()
    {
        if (UserSearchConfig.EmailIntervalDays is null)
        {
            UserSearchConfig.EmailIntervalDays = new();
        }

        UserSearchConfig.EmailIntervalDays.Add(new());
    }

    private void HandleRemoveEmailIntervalDay(EmailIntervalDay emailIntervalDay)
    {
        UserSearchConfig.EmailIntervalDays!.Remove(emailIntervalDay);
    }
}