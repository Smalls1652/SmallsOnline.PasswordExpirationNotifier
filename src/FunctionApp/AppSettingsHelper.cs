namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

/// <summary>
/// Helper class for reading app settings from environment variables.
/// </summary>
public static class AppSettingsHelper
{
    /// <summary>
    /// Get the value of an environment variable.
    /// </summary>
    /// <param name="settingName">The name of the environment variable the setting value is stored in.</param>
    /// <returns>The setting value.</returns>
    /// <exception cref="NullReferenceException">No environment variable matched the supplied setting name.</exception>
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