using System;
using System.Configuration;

namespace Quartz.AttributesExtension.Configuration
{
    internal sealed class ConfigurationProvider : IConfigurationProvider
    {
        public string GetString(string key) => ConfigurationManager.AppSettings[key];

        public int? GetInt(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            return value == null ? (int?)null : Convert.ToInt32(value);
        }

        public bool? GetBool(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            return value == null ? (bool?)null : Convert.ToBoolean(value);
        }
    }
}
