﻿using FluentAssertions;
using Moq;
using Quartz.AttributesExtension.JobData;
using Quartz.AttributesExtension.Trigger;
using System;
using Xunit;

namespace Quartz.AttributesExtension
{
    public sealed class JobSchedulderTests : IDisposable
    {
        private readonly Mock<IScheduler> schedulerMock = new Mock<IScheduler>(MockBehavior.Strict);
        private readonly Mock<IJobDataBuilder> jobDataBuilderMock = new Mock<IJobDataBuilder>(MockBehavior.Strict);
        private readonly Mock<ITriggerBuilderFactory> triggerBuilderFactoryMock = new Mock<ITriggerBuilderFactory>(MockBehavior.Strict);
        private readonly Mock<ITriggerBuilder> triggerBuilderMock = new Mock<ITriggerBuilder>(MockBehavior.Strict);
        private readonly JobScheduler subject;

        public void Dispose()
        {
            this.schedulerMock.VerifyAll();
            this.jobDataBuilderMock.VerifyAll();
            this.triggerBuilderFactoryMock.VerifyAll();
        }

        public JobSchedulderTests()
        {
            this.subject = new JobScheduler(schedulerMock.Object, jobDataBuilderMock.Object, triggerBuilderFactoryMock.Object);
        }

        [Fact]
        public void ScheduleJob()
        {
            // ARRANGE
            var jobKey = new JobKey(nameof(SampleJob));
            var jobDataMap = new JobDataMap
            {
                { "BoolParam", true }
            };
            var triggerMock = new Mock<ITrigger>().Object;

            this.jobDataBuilderMock.Setup(m => m.Build(typeof(SampleJob), jobKey))
                .Returns(jobDataMap);
            this.triggerBuilderFactoryMock.Setup(m => m.GetTriggerBuilder(It.IsAny<SimpleTriggerFromConfigAttribute>()))
                .Returns(triggerBuilderMock.Object);
            this.triggerBuilderMock.Setup(m => m.BuildTrigger(It.IsAny<SimpleTriggerFromConfigAttribute>(), It.IsAny<JobKey>()))
                .Returns(triggerMock);
            
            this.schedulerMock.Setup(m => m.AddJob(It.IsAny<IJobDetail>(), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, jobKey, jobDataMap));
            
            this.schedulerMock.Setup(m => m.ScheduleJob(triggerMock)).Returns(new DateTimeOffset());

            // ACT
            this.subject.ScheduleJob<SampleJob>();

            // ASSERT
        }

        [Fact]
        public void ScheduleJobWithDefaultName()
        {
            // ARRANGE
            var jobKey = new JobKey(nameof(SampleJob2));
            var jobDataMap = new JobDataMap();

            this.jobDataBuilderMock.Setup(m => m.Build(typeof(SampleJob2), jobKey))
                .Returns(jobDataMap);

            this.schedulerMock.Setup(m => m.AddJob(It.IsAny<IJobDetail>(), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, jobKey, jobDataMap));

            // ACT
            this.subject.ScheduleJob<SampleJob2>();

            // ASSERT
        }

        [Fact]
        public void ScheduleAllJobs()
        {
            // ARRANGE
            var job1Key = new JobKey(nameof(SampleJob));
            var job2Key = new JobKey(nameof(SampleJob2));
            var triggerMock = new Mock<ITrigger>().Object;
            var emptyJobDataMap = new JobDataMap();

            this.jobDataBuilderMock.Setup(m => m.Build(typeof(SampleJob), job1Key))
                .Returns(emptyJobDataMap);
            this.jobDataBuilderMock.Setup(m => m.Build(typeof(SampleJob2), job2Key))
                .Returns(emptyJobDataMap);

            this.triggerBuilderFactoryMock.Setup(m => m.GetTriggerBuilder(It.IsAny<SimpleTriggerFromConfigAttribute>()))
                .Returns(triggerBuilderMock.Object);
            this.triggerBuilderMock.Setup(m => m.BuildTrigger(It.IsAny<SimpleTriggerFromConfigAttribute>(), It.IsAny<JobKey>()))
                .Returns(triggerMock);

            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(SampleJob)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job1Key, emptyJobDataMap));

            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(SampleJob2)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job2Key, emptyJobDataMap));

            this.schedulerMock.Setup(m => m.ScheduleJob(triggerMock)).Returns(new DateTimeOffset());

            // ACT
            this.subject.ScheduleAll();
        }

        private static void VerifyJobDetail(IJobDetail jobDetail, JobKey expectedJobKey, JobDataMap expectedJobDataMap)
        {
            jobDetail.Should().NotBeNull();
            jobDetail.Key.Name.Should().Be(expectedJobKey.Name);
            jobDetail.Key.Group.Should().Be(expectedJobKey.Group);
            jobDetail.JobDataMap.Should().BeEquivalentTo(expectedJobDataMap);
        }
    }
}
