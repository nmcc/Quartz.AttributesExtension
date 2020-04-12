using Common.Logging;
using FluentAssertions;
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
        private readonly Mock<ILog> logMock = new Mock<ILog>();

        private readonly JobScheduler subject;

        public void Dispose()
        {
            this.schedulerMock.VerifyAll();
            this.jobDataBuilderMock.VerifyAll();
            this.triggerBuilderFactoryMock.VerifyAll();
            this.logMock.VerifyAll();
        }

        public JobSchedulderTests()
        {
            this.subject = new JobScheduler(schedulerMock.Object, jobDataBuilderMock.Object, triggerBuilderFactoryMock.Object, logMock.Object);
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
            this.triggerBuilderMock.Setup(m => m.BuildTrigger(It.IsAny<SimpleTriggerFromConfigAttribute>(), It.IsAny<JobKey>(), typeof(SampleJob)))
                .Returns(triggerMock);
            this.schedulerMock.Setup(m => m.ScheduleJob(triggerMock))
                .Returns(new DateTimeOffset());

            // ARRANGE and ASSERT
            this.schedulerMock.Setup(m => m.AddJob(It.IsAny<IJobDetail>(), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, jobKey, jobDataMap));

            this.logMock.Setup(m => m.Info(It.IsAny<string>()))
                .Callback((object message) => (message as string)?.Contains(nameof(SampleJob)));

            // ACT
            this.subject.ScheduleJob<SampleJob>();
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
            this.triggerBuilderMock.Setup(m => m.BuildTrigger(It.IsAny<SimpleTriggerFromConfigAttribute>(), It.IsAny<JobKey>(), typeof(SampleJob)))
                .Returns(triggerMock);
            this.schedulerMock.Setup(m => m.ScheduleJob(triggerMock))
                .Returns(new DateTimeOffset());

            // ARRANGE and ASSERT
            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(SampleJob)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job1Key, emptyJobDataMap));

            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(SampleJob2)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job2Key, emptyJobDataMap));

            // ACT
            this.subject.ScheduleAll();
        }

        private static void VerifyJobDetail(IJobDetail jobDetail, JobKey expectedJobKey, JobDataMap expectedJobDataMap)
        {
            jobDetail.Should().NotBeNull();
            jobDetail.Key.Should().BeEquivalentTo(expectedJobKey);
            jobDetail.JobDataMap.Should().BeEquivalentTo(expectedJobDataMap);
        }
    }
}
