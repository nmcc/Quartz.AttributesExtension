using FluentAssertions;
using Moq;
using Quartz.AttributesExtension.Configuration;
using Quartz.Impl.Triggers;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public class SimpleTriggerFromConfigAttributeBuilderTests
    {
        private readonly JobKey jobKey = new JobKey("job1");
        private readonly Mock<IConfigurationProvider> configurationProviderMock;
        private readonly SimpleTriggerFromConfigAttributeBuilder subject;

        public SimpleTriggerFromConfigAttributeBuilderTests()
        {
            this.configurationProviderMock = new Mock<IConfigurationProvider>();
            this.subject = new SimpleTriggerFromConfigAttributeBuilder(configurationProviderMock.Object);
        }

        [Fact]
        public void HappyPath()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.IntervalInSeconds")).Returns(10);

            // ACT
            var trigger = subject.BuildTrigger(new SimpleTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<SimpleTriggerImpl>();

            var simpleTrigger = trigger as SimpleTriggerImpl;
            simpleTrigger.JobKey.Should().Be(jobKey);
            simpleTrigger.RepeatInterval.Should().Be(TimeSpan.FromSeconds(10));
            simpleTrigger.RepeatCount.Should().Be(0); 
        }

        [Fact]
        public void NullInterval()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.IntervalInSeconds")).Returns((int?)null);

            // ACT
            Action action = () => subject.BuildTrigger(new SimpleTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            action.Should().Throw<InvalidQuartzConfigurationException>()
                .And.Message.Should().Contain("Jobs.job1.trigger1.IntervalInSeconds");
        }
    }
}
