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
            public const string RepeatCount = "RepeatCount";
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

            var (interval, repeatForever, repeatCount) = GetParameters(triggerKey);

            if (repeatForever == true)
            {
                triggerBuilder.WithSimpleSchedule(t => t.WithInterval(TimeSpan.FromSeconds(interval)).RepeatForever());
            }
            else
            {
                triggerBuilder.WithSimpleSchedule(t => t
                    .WithInterval(TimeSpan.FromSeconds(interval))
                    .WithRepeatCount(repeatCount));
            }

            return triggerBuilder.Build();
        }

        private (int interval, bool? repeatForever, int repeatCount) GetParameters(TriggerKey triggerKey)
        {
            var intervalKey = ConfigurationKeyBuilder.Build(triggerKey, ConfigurationParamaters.Interval);
            var interval = this.configurationProvider.GetInt(intervalKey)
                ?? throw new InvalidQuartzConfigurationException($"Unable to get interval from configuration key {intervalKey}");

            var repeatForeverKey = ConfigurationKeyBuilder.Build(triggerKey, ConfigurationParamaters.RepeatForever);
            var repeatForever = this.configurationProvider.GetBool(repeatForeverKey);

            var repeatCount = default(int);

            if (!repeatForever.HasValue)
            {
                var repeatCountKey = ConfigurationKeyBuilder.Build(triggerKey, ConfigurationParamaters.RepeatCount);

                repeatCount = this.configurationProvider.GetInt(repeatCountKey)
                    ?? throw new InvalidQuartzConfigurationException($"Unable to determine whether the job should repeat forever or on a determined count. Please make sure either {repeatForeverKey} or {repeatCountKey} are defined.");
            }

            return (interval, repeatForever, repeatCount);
        }
    }
}
