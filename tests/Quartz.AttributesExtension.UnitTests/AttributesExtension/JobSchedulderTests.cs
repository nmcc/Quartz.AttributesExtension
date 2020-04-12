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
        private readonly Mock<ITrigger> triggerMock = new Mock<ITrigger>();

        private readonly JobScheduler subject;

        private readonly JobKey job1Key = new JobKey(nameof(SampleJob));
        private readonly JobKey job2Key = new JobKey(nameof(SampleJob2));
        private readonly JobKey job3Key = new JobKey(nameof(InterruptableJob));

        private readonly JobDataMap emptyDataMap = new JobDataMap();
        private readonly JobDataMap job1DataMap = new JobDataMap { { "BoolParam", true } };


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
            SetupMocks_SampleJob();

            // ARRANGE and ASSERT
            this.logMock.Setup(m => m.Info(It.IsAny<string>()))
                .Callback((object message) => (message as string)?.Contains(nameof(SampleJob)));

            // ACT
            this.subject.ScheduleJob<SampleJob>();
        }

        [Fact]
        public void ScheduleAllJobs()
        {
            // ARRANGE
            SetupMocks_SampleJob();
            SetupMocks_SampleJob2();
            SetupMocks_InterruptableJob();

            // ACT
            this.subject.ScheduleAll();
        }

        private void SetupMocks_SampleJob()
        {
            this.jobDataBuilderMock.Setup(m => m.Build(typeof(SampleJob), job1Key))
                .Returns(job1DataMap);

            this.triggerBuilderFactoryMock.Setup(m => m.GetTriggerBuilder(It.IsAny<SimpleTriggerFromConfigAttribute>()))
                .Returns(triggerBuilderMock.Object);

            this.triggerBuilderMock.Setup(m => m.BuildTrigger(It.IsAny<SimpleTriggerFromConfigAttribute>(), It.IsAny<JobKey>(), typeof(SampleJob)))
                .Returns(triggerMock.Object);

            this.schedulerMock.Setup(m => m.ScheduleJob(triggerMock.Object))
              .Returns(new DateTimeOffset());

            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(SampleJob)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job1Key, job1DataMap));
        }

        private void SetupMocks_SampleJob2()
        {
            this.jobDataBuilderMock.Setup(m => m.Build(typeof(SampleJob2), job2Key))
                .Returns(emptyDataMap);

            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(SampleJob2)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job2Key, emptyDataMap));
        }

        private void SetupMocks_InterruptableJob()
        {
            this.jobDataBuilderMock.Setup(m => m.Build(typeof(InterruptableJob), job3Key))
                .Returns(emptyDataMap);

            this.schedulerMock.Setup(m => m.AddJob(It.Is<IJobDetail>(j => j.Key.Name == nameof(InterruptableJob)), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail, job3Key, emptyDataMap));
        }

        private static void VerifyJobDetail(IJobDetail jobDetail, JobKey expectedJobKey, JobDataMap expectedJobDataMap)
        {
            jobDetail.Should().NotBeNull();
            jobDetail.Key.Should().BeEquivalentTo(expectedJobKey);
            jobDetail.JobDataMap.Should().BeEquivalentTo(expectedJobDataMap);
        }
    }
}
