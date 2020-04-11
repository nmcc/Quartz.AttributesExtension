using Common.Logging;
using Quartz.AttributesExtension.JobData;
using Quartz.AttributesExtension.Reflection;
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
        private readonly ILog logger;

        public JobScheduler(IScheduler scheduler)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.jobDataBuilder = new JobDataBuilder();
            this.triggerBuilderFactory = new TriggerBuilderFactory();
            this.logger = LogManager.GetCurrentClassLogger();
        }

        internal JobScheduler(IScheduler scheduler, IJobDataBuilder jobDataBuilder, ITriggerBuilderFactory triggerBuilderFactory, ILog logger)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.jobDataBuilder = jobDataBuilder ?? throw new ArgumentNullException(nameof(jobDataBuilder));
            this.triggerBuilderFactory = triggerBuilderFactory ?? throw new ArgumentNullException(nameof(triggerBuilderFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void ScheduleAll()
        {
            foreach (var jobType in JobLocator.GetAllJobsInAppDomain())
            {
                this.ScheduleJob(jobType);
            }
        }

        public void ScheduleJob<JobType>() where JobType : IJob
        {
            this.ScheduleJob(typeof(JobType));
        }

        private void ScheduleJob(Type jobType)
        {
            var jobAttributes = jobType.GetCustomAttributes(typeof(JobAttribute), inherit: false)
                .Cast<JobAttribute>();

            var triggerAttributes = jobType.GetCustomAttributes(typeof(ITriggerAttribute), inherit: false)
                .Cast<ITriggerAttribute>();

            ScheduleJob(scheduler, jobType, jobAttributes.First(), triggerAttributes);
        }

        private void ScheduleJob(IScheduler scheduler, Type jobType, JobAttribute jobAttribute, IEnumerable<ITriggerAttribute> triggerAttributes)
        {
            var jobKey = BuildJobKey(jobAttribute, jobType);

            var jobDataMap = this.jobDataBuilder.Build(jobType, jobKey);

            var job = JobBuilder.Create(jobType)
                .WithIdentity(jobKey)
                .SetJobData(jobDataMap)
                .StoreDurably()
                .Build();

            scheduler.AddJob(job, replace: true);

            if (logger.IsDebugEnabled)
            {
                logger.Debug($"Job {jobKey}: added to scheduler with {jobDataMap.Count} elements on Job Data");
            }

            foreach (var triggerAttr in triggerAttributes)
            {
                var builder = this.triggerBuilderFactory.GetTriggerBuilder(triggerAttr)
                    ?? throw new InvalidOperationException($"No trigger builder for trigger type {triggerAttr.GetType().FullName}");

                var trigger = builder.BuildTrigger(triggerAttr, jobKey)
                    ?? throw new InvalidQuartzConfigurationException($"Unable to build trigger {triggerAttr.Group}.{triggerAttr.Name}");

                scheduler.ScheduleJob(trigger);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug($"Job {jobKey}: Added {trigger.Key} trigger");
                }
            }

            logger.Info($"Job {jobKey} is scheduled");
        }

        private static JobKey BuildJobKey(JobAttribute jobAttribute, Type jobType)
        {
            if (string.IsNullOrWhiteSpace(jobAttribute.Name))
                return new JobKey(jobType.Name);
            else if (jobAttribute.Group == null)
                return new JobKey(jobAttribute.Name);

            return new JobKey(jobAttribute.Name, jobAttribute.Group);
        }
    }
}
