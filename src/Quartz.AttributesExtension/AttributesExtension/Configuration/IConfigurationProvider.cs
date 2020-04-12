namespace Quartz.AttributesExtension.Configuration
{
    internal interface IConfigurationProvider
    {
        bool? GetBool(string key);

        int? GetInt(string key);

        string GetString(string key);
    }
}