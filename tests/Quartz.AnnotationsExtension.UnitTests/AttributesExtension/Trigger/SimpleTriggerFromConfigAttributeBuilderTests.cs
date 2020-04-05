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
        public void RepeatForever()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.IntervalInSeconds")).Returns(10);
            configurationProviderMock.Setup(m => m.GetBool("Jobs.job1.trigger1.RepeatForever")).Returns(true);

            // ACT
            var trigger = subject.BuildTrigger(new SimpleTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<SimpleTriggerImpl>();

            var simpleTrigger = trigger as SimpleTriggerImpl;
            simpleTrigger.JobKey.Should().Be(jobKey);
            simpleTrigger.RepeatInterval.Should().Be(TimeSpan.FromSeconds(10));
            simpleTrigger.RepeatCount.Should().Be(-1);
        }

        [Fact]
        public void RepeatCount()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.IntervalInSeconds")).Returns(10);
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.RepeatCount")).Returns(100);

            // ACT
            var trigger = subject.BuildTrigger(new SimpleTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<SimpleTriggerImpl>();

            var simpleTrigger = trigger as SimpleTriggerImpl;
            simpleTrigger.JobKey.Should().Be(jobKey);
            simpleTrigger.RepeatInterval.Should().Be(TimeSpan.FromSeconds(10));
            simpleTrigger.RepeatCount.Should().Be(100);
        }

        [Fact]
        public void NullIntervalInSeconds()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.IntervalInSeconds")).Returns((int?)null);

            // ACT
            Action action = () => subject.BuildTrigger(new SimpleTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            action.Should().ThrowExactly<InvalidQuartzConfigurationException>();
        }

        [Fact]
        public void MissingRepeatCountAndRepeatForever()
        {
            // ARRANGE
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.IntervalInSeconds")).Returns(10);
            configurationProviderMock.Setup(m => m.GetInt("Jobs.job1.trigger1.RepeatCount")).Returns((int?)null);
            configurationProviderMock.Setup(m => m.GetBool("Jobs.job1.trigger1.RepeatForever")).Returns((bool?)null);

            // ACT
            Action action = () => subject.BuildTrigger(new SimpleTriggerFromConfigAttribute("trigger1"), jobKey);

            // ASSERT
            var exception = action.Should().Throw<InvalidQuartzConfigurationException>().And;
            exception.Message.Should().Contain("Jobs.job1.trigger1.RepeatForever");
            exception.Message.Should().Contain("Jobs.job1.trigger1.RepeatCount");
        }
    }
}
