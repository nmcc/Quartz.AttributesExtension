using System;
using static Quartz.AttributesExtension.Trigger.TriggerKeyBuilder;

namespace Quartz.AttributesExtension.Trigger
{
    internal class CronTriggerAttributeBuilder : ITriggerBuilder
    {
        public ITrigger BuildTrigger(ITriggerAttribute triggerAttribute, JobKey jobKey, Type jobType)
        {
            var cronTrigger = triggerAttribute as CronTriggerAttribute
                ?? throw new ArgumentException($"{nameof(triggerAttribute)} must be of type CronTriggerAttribute");

            var triggerKey = BuildTriggerKey(cronTrigger, jobType);

            // Trigger the job to run now, and then repeat every 10 seconds
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .ForJob(jobKey)
                .WithCronSchedule(cronTrigger.Expression);

            return triggerBuilder.Build();
        }
    }
}
