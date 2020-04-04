using FluentAssertions;
using Moq;
using Quartz.AttributesExtension.Configuration;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public class CronTriggerFromConfigAttributeBuilderTests
    {
        private const string CronExpression = "0/10 * * * * ? *";
        private readonly JobKey jobKey = new JobKey("job1");
        private readonly Mock<IConfigurationProvider> configurationProviderMock;
        private readonly CronTriggerFromConfigAttributeBuilder subject;

        public CronTriggerFromConfigAttributeBuilderTests()
        {
            this.configurationProviderMock = new Mock<IConfigurationProvider>();
            this.subject = new CronTriggerFromConfigAttributeBuilder(configurationProviderMock.Object);
        }

        [Fact]
        public void HappyPath()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetString("Jobs.job1.trigger1.Cron")).Returns(CronExpression);

            // ACT
            var trigger = subject.BuildTrigger(new CronTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.JobKey.Should().Be(jobKey);
            // Unable to test the cron expression :-(
        }

        [Fact]
        public void NullCronExpression()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetString("Jobs.job1.trigger1.Cron")).Returns((string)null);

            // ACT
            Action action = () => subject.BuildTrigger(new CronTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            action.Should().Throw<InvalidQuartzConfigurationException>()
                .And.Message.Should().Contain("Jobs.job1.trigger1.Cron");
        }
    }
}
