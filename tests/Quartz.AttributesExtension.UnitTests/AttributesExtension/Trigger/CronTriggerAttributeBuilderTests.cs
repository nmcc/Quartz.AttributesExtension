using FluentAssertions;
using Quartz.Impl.Triggers;
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
            var trigger = subject.BuildTrigger(new CronTriggerAttribute(CronExpression), jobKey, this.GetType());

            // ASSERT
            trigger.Should().NotBeNull();
            trigger.Should().BeOfType<CronTriggerImpl>();

            var cronTrigger = trigger as CronTriggerImpl;
            cronTrigger.JobKey.Should().Be(jobKey);
            cronTrigger.CronExpressionString.Should().Be(CronExpression);
        }
    }
}
