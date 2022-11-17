using Quartz.AttributesExtension.Configuration;
using System;
using static Quartz.AttributesExtension.Trigger.TriggerKeyBuilder;

namespace Quartz.AttributesExtension.Trigger
{
    internal sealed class CronTriggerFromConfigAttributeBuilder : ITriggerBuilder
    {
        private static class ConfigurationParamaters
        {
            public const string Cron = "Cron";
        }

        private readonly IConfigurationProvider configurationProvider;

        public CronTriggerFromConfigAttributeBuilder()
        {
            this.configurationProvider = new ConfigurationProvider();
        }

        public CronTriggerFromConfigAttributeBuilder(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public ITrigger BuildTrigger(ITriggerAttribute triggerAttribute, JobKey jobKey, Type jobType)
        {
            var cronTrigger = triggerAttribute as CronTriggerFromConfigAttribute
                ?? throw new ArgumentException($"{nameof(triggerAttribute)} must be of type CronTriggerFromConfigAttribute");

            var triggerKey = BuildTriggerKey(cronTrigger, jobType);

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(jobKey);

            var cronExpressionKey = ConfigurationKeyBuilder.Build(triggerKey, ConfigurationParamaters.Cron);
            var cronExpression = this.configurationProvider.GetString(cronExpressionKey);

            if (cronExpression == null)
            {
                return null;
            }

            triggerBuilder.WithCronSchedule(cronExpression);

            return triggerBuilder.Build();
        }
    }
}
