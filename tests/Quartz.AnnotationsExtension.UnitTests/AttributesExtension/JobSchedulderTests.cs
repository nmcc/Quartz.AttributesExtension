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
        public void HappyPath()
        {
            // ARRANGE
            var jobKey = new JobKey(nameof(SampleJob));
            var jobDataMap = new JobDataMap
            {
                { "BoolParam", true }
            };
            var triggerMock = new Mock<ITrigger>().Object;

            this.jobDataBuilderMock.Setup(m => m.Build<SampleJob>(jobKey))
                .Returns(jobDataMap);
            this.triggerBuilderFactoryMock.Setup(m => m.GetTriggerBuilder(It.IsAny<SimpleTriggerFromConfigAttribute>()))
                .Returns(triggerBuilderMock.Object);
            this.triggerBuilderMock.Setup(m => m.BuildTrigger(It.IsAny<SimpleTriggerFromConfigAttribute>(), It.IsAny<JobKey>()))
                .Returns(triggerMock);
            
            this.schedulerMock.Setup(m => m.AddJob(It.IsAny<IJobDetail>(), true))
                .Callback((IJobDetail jobDetail, bool _) => VerifyJobDetail(jobDetail));
            
            this.schedulerMock.Setup(m => m.ScheduleJob(triggerMock)).Returns(new DateTimeOffset());

            // ACT
            this.subject.ScheduleJob<SampleJob>();

            // ASSERT
            void VerifyJobDetail(IJobDetail jobDetail)
            {
                jobDetail.Should().NotBeNull();
                jobDetail.Key.Should().Be(jobKey);
                jobDetail.JobDataMap.Should().BeSameAs(jobDataMap);
            }
        }
    }
}
