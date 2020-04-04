using Quartz.AttributesExtension.JobData;
using Quartz.AttributesExtension.Trigger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quartz.AttributesExtension
{
    internal sealed class JobScheduler
    {
        private readonly IScheduler scheduler;
        private readonly IJobDataBuilder jobDataBuilder;
        private readonly ITriggerBuilderFactory triggerBuilderFactory;

        public JobScheduler(IScheduler scheduler)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.jobDataBuilder = new JobDataBuilder();
            this.triggerBuilderFactory = new TriggerBuilderFactory();
        }

        internal JobScheduler(IScheduler scheduler, IJobDataBuilder jobDataBuilder, ITriggerBuilderFactory triggerBuilderFactory)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.jobDataBuilder = jobDataBuilder ?? throw new ArgumentNullException(nameof(jobDataBuilder));
            this.triggerBuilderFactory = triggerBuilderFactory ?? throw new ArgumentNullException(nameof(triggerBuilderFactory));
        }

        public void ScheduleJob<JobType>() where JobType : IJob
        {
            var jobAttributes = typeof(JobType).GetCustomAttributes(typeof(JobAttribute), inherit: false)
                .Cast<JobAttribute>();

            var triggerAttributes = typeof(JobType).GetCustomAttributes(typeof(ITriggerAttribute), inherit: false)
                .Cast<ITriggerAttribute>();

            ScheduleJob<JobType>(scheduler, jobAttributes.First(), triggerAttributes);
        }

        private void ScheduleJob<JobType>(IScheduler scheduler, JobAttribute jobAttribute, IEnumerable<ITriggerAttribute> triggerAttributes) where JobType : IJob
        {
            var jobKey = BuildJobKey(jobAttribute);

            var jobDataMap = this.jobDataBuilder.Build<JobType>(jobKey);

            var job = JobBuilder.Create<JobType>()
                .WithIdentity(jobKey)
                .SetJobData(jobDataMap)
                .StoreDurably()
                .Build();

            scheduler.AddJob(job, replace: true);

            foreach (var triggerAttr in triggerAttributes)
            {
                var builder = this.triggerBuilderFactory.GetTriggerBuilder(triggerAttr)
                    ?? throw new InvalidOperationException($"No trigger builder for trigger type {triggerAttr.GetType().FullName}");

                var trigger = builder.BuildTrigger(triggerAttr, jobKey)
                    ?? throw new InvalidQuartzConfigurationException($"Unable to build trigger {triggerAttr.Group}.{triggerAttr.Name}");

                scheduler.ScheduleJob(trigger);
            }
        }

        private static JobKey BuildJobKey(JobAttribute jobAttribute)
            => jobAttribute.Group == null
                ? new JobKey(jobAttribute.Name)
                : new JobKey(jobAttribute.Name, jobAttribute.Group);
    }
}
