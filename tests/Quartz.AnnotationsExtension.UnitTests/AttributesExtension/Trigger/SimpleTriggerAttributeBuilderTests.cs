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
        public void HappyPath()
        {
            // ACT
            var trigger = subject.BuildTrigger(new SimpleTriggerAttribute(0, 0, 0, 1, true), jobKey);

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<SimpleTriggerImpl>();

            var simpleTrigger = trigger as SimpleTriggerImpl;
            simpleTrigger.JobKey.Should().Be(jobKey);
            simpleTrigger.RepeatInterval.Should().Be(TimeSpan.FromSeconds(1));
            simpleTrigger.RepeatCount.Should().Be(-1); // Forever
        }
    }
}
