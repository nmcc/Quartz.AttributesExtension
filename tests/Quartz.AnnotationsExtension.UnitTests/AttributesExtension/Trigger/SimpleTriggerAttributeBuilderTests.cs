using FluentAssertions;
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
            trigger.JobKey.Should().Be(jobKey);
            // Unable to test the cron expression :-(
        }
    }
}
