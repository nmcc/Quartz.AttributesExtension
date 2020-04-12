using FluentAssertions;
using Quartz.Impl.Triggers;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public class SimpleTriggerAttributeBuilderTests
    {
        private readonly SimpleTriggerAttributeBuilder subject = new SimpleTriggerAttributeBuilder();
        private readonly JobKey jobKey = new JobKey("job1");

        [Fact]
        public void RepeatForever()
        {
            // ACT
            var trigger = subject.BuildTrigger(new SimpleTriggerAttribute(0, 0, 0, 1), jobKey, this.GetType());

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<SimpleTriggerImpl>();

            var simpleTrigger = trigger as SimpleTriggerImpl;
            simpleTrigger.JobKey.Should().Be(jobKey);
            simpleTrigger.RepeatInterval.Should().Be(TimeSpan.FromSeconds(1));
            simpleTrigger.RepeatCount.Should().Be(-1); // Forever
        }

        [Fact]
        public void RepeatCount()
        {
            // ACT
            var trigger = subject.BuildTrigger(new SimpleTriggerAttribute(0, 0, 0, 1, repeatCount: 100), jobKey, this.GetType());

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<SimpleTriggerImpl>();

            var simpleTrigger = trigger as SimpleTriggerImpl;
            simpleTrigger.JobKey.Should().Be(jobKey);
            simpleTrigger.RepeatInterval.Should().Be(TimeSpan.FromSeconds(1));
            simpleTrigger.RepeatCount.Should().Be(100);
        }
    }
}
