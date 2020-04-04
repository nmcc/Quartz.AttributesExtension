using FluentAssertions;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public class CronTriggerAttributeBuilderTests
    {
        private const string CronExpression = "0/10 * * * * ? *";
        private readonly CronTriggerAttributeBuilder subject = new CronTriggerAttributeBuilder();
        private readonly JobKey jobKey = new JobKey("job1");

        [Fact]
        public void HappyPath()
        {
            // ACT
            var trigger = subject.BuildTrigger(new CronTriggerAttribute(CronExpression), jobKey);

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.JobKey.Should().Be(jobKey);
            // Unable to test the cron expression :-(
        }
    }
}
