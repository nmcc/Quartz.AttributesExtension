using System;
using static Quartz.AttributesExtension.Trigger.TriggerKeyBuilder;

namespace Quartz.AttributesExtension.Trigger
{
    internal sealed class SimpleTriggerAttributeBuilder : ITriggerBuilder
    {
        public ITrigger BuildTrigger(ITriggerAttribute triggerAttribute, JobKey jobKey, Type jobType)
        {
            var simpleTrigger = triggerAttribute as SimpleTriggerAttribute
                ?? throw new ArgumentException($"{nameof(triggerAttribute)} must be of type SimpleTriggerAttribute");

            var triggerKey = BuildTriggerKey(simpleTrigger, jobType);

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(jobKey);

            if (simpleTrigger.RepeatForever)
            {
                triggerBuilder.WithSimpleSchedule(t => t
                    .WithInterval(simpleTrigger.Interval)
                    .RepeatForever());
            }
            else
            {
                triggerBuilder.WithSimpleSchedule(t => t
                    .WithInterval(simpleTrigger.Interval)
                    .WithRepeatCount(simpleTrigger.RepeatCount));
            }

            return triggerBuilder.Build();
        }
    }
}
