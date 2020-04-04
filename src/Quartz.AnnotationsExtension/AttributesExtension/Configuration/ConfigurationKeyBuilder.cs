using System;
using System.Collections.Generic;

namespace Quartz.AttributesExtension.Configuration
{
    internal static class ConfigurationKeyBuilder
    {
        // TODO: Make this configurable
        private const string ConfigurationPrefix = "Jobs";

        public static string Build(JobKey jobKey, TriggerKey triggerKey, params string[] parameters)
        {
            if (jobKey == null) throw new ArgumentNullException(nameof(jobKey));
            if (triggerKey == null) throw new ArgumentNullException(nameof(triggerKey));

            var keys = new List<string>();

            AddKeys(keys, jobKey);

            AddKeys(triggerKey, keys);

            AddKeys(keys, parameters);

            return string.Join(".", keys);
        }

        public static string Build(JobKey jobKey, params string[] parameters)
        {
            if (jobKey == null) throw new ArgumentNullException(nameof(jobKey));

            var keys = new List<string>();

            AddKeys(keys, jobKey);
            AddKeys(keys, parameters);

            return string.Join(".", keys);
        }

        private static void AddKeys(List<string> keys, JobKey jobKey)
        {
            keys.Add(ConfigurationPrefix);

            if (jobKey.Group != QuartzConstants.DefaultGroup)
                keys.Add(jobKey.Group);

            keys.Add(jobKey.Name);
        }

        private static void AddKeys(TriggerKey triggerKey, List<string> keys)
        {
            if (triggerKey.Group != QuartzConstants.DefaultGroup)
                keys.Add(triggerKey.Group);

            keys.Add(triggerKey.Name);
        }

        private static void AddKeys(List<string> keys, string[] parameters)
        {
            if (parameters != null)
            {
                keys.AddRange(parameters);
            }
        }
    }
}
