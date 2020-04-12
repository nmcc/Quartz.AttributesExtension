using FluentAssertions;
using Moq;
using Quartz.AttributesExtension.Configuration;
using Quartz.Impl.Triggers;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public class CronTriggerFromConfigAttributeBuilderTests : IDisposable
    {
        private const string CronExpression = "0/10 * * * * ? *";
        private readonly JobKey jobKey = new JobKey("job1");
        private readonly Mock<IConfigurationProvider> configurationProviderMock;
        private readonly CronTriggerFromConfigAttributeBuilder subject;

        public CronTriggerFromConfigAttributeBuilderTests()
        {
            this.configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            this.subject = new CronTriggerFromConfigAttributeBuilder(configurationProviderMock.Object);
        }

        public void Dispose()
        {
            this.configurationProviderMock.VerifyAll();
        }

        [Fact]
        public void HappyPath()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetString("Quartz.Triggers.trigger1.Cron")).Returns(CronExpression);

            // ACT
            var trigger = subject.BuildTrigger(new CronTriggerFromConfigAttribute("trigger1"), jobKey, this.GetType());

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<CronTriggerImpl>();

            var cronTrigger = trigger as CronTriggerImpl;
            cronTrigger.JobKey.Should().Be(jobKey);
            cronTrigger.CronExpressionString.Should().Be(CronExpression);
        }

        [Fact]
        public void DefaultCtor()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetString("Quartz.Triggers.CronTriggerFromConfigAttributeBuilderTests.Cron")).Returns(CronExpression);

            // ACT
            var trigger = subject.BuildTrigger(new CronTriggerFromConfigAttribute(), jobKey, this.GetType());

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<CronTriggerImpl>();

            var cronTrigger = trigger as CronTriggerImpl;
            cronTrigger.JobKey.Should().Be(jobKey);
            cronTrigger.CronExpressionString.Should().Be(CronExpression);
        }

        [Fact]
        public void NullCronExpression()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetString("Quartz.Triggers.trigger1.Cron")).Returns((string)null);

            // ACT
            Action action = () => subject.BuildTrigger(new CronTriggerFromConfigAttribute("trigger1"), jobKey, this.GetType());

            // ASSERT
            action.Should().Throw<InvalidQuartzConfigurationException>()
                .And.Message.Should().Contain("Quartz.Triggers.trigger1.Cron");
        }
    }
}
