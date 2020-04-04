using Quartz.AttributesExtension.Configuration;
using System;
using static Quartz.AttributesExtension.Trigger.TriggerKeyBuilder;

namespace Quartz.AttributesExtension.Trigger
{
    internal sealed class SimpleTriggerFromConfigAttributeBuilder : ITriggerBuilder
    {
        private static class ConfigurationParamaters
        {
            public const string Interval = "IntervalInSeconds";
            public const string RepeatForever = "RepeatForever";
        }

        private readonly IConfigurationProvider configurationProvider;

        public SimpleTriggerFromConfigAttributeBuilder()
        {
            this.configurationProvider = new ConfigurationProvider();
        }

        public SimpleTriggerFromConfigAttributeBuilder(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public ITrigger BuildTrigger(ITriggerAttribute triggerAttribute, JobKey jobKey)
        {
            var simpleTrigger = triggerAttribute as SimpleTriggerFromConfigAttribute
               ?? throw new ArgumentException($"{nameof(triggerAttribute)} must be of type SimpleTriggerFromConfigAttribute");

            var triggerKey = BuildTriggerKey(simpleTrigger);

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(jobKey);

            var intervalKey = ConfigurationKeyBuilder.Build(jobKey, triggerKey, ConfigurationParamaters.Interval);
            var interval = this.configurationProvider.GetInt(intervalKey)
                ?? throw new InvalidQuartzConfigurationException($"Unable to get interval from configuration key {intervalKey}");

            var repeatForeverKey = ConfigurationKeyBuilder.Build(jobKey, triggerKey, ConfigurationParamaters.RepeatForever);
            var repeatForever = this.configurationProvider.GetBool(repeatForeverKey);

            if (repeatForever == true)
            {
                triggerBuilder.WithSimpleSchedule(t => t.WithInterval(TimeSpan.FromSeconds(interval)).RepeatForever());
            }
            else
            {
                triggerBuilder.WithSimpleSchedule(t => t.WithInterval(TimeSpan.FromSeconds(interval)));
            }

            return triggerBuilder.Build();
        }
    }
}
