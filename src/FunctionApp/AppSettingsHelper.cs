namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

public static class AppSettingsHelper
{
    public static string GetSettingValue(string settingName)
    {
        string? settingValue = Environment.GetEnvironmentVariable(settingName);

        if (settingValue is null)
        {
            throw new NullReferenceException($"Setting value for {settingValue} is null.");
        }

        return settingValue;
    }
}