using FluentAssertions;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public class TriggerBuilderFactoryTests
    {
        private readonly TriggerBuilderFactory subject = new TriggerBuilderFactory();

        [Fact]
        public void TriggerBuilderTest_CronTriggerAttribute()
        {
            // ACT
            var triggerBuilder = subject.GetTriggerBuilder(new CronTriggerAttribute("some expression"));

            // ASSERT
            triggerBuilder.Should().NotBeNull();
            triggerBuilder.Should().BeOfType<CronTriggerAttributeBuilder>();
        }

        [Fact]
        public void TriggerBuilderTest_SimpleTriggerAttribute()
        {
            // ACT
            var triggerBuilder = subject.GetTriggerBuilder(new SimpleTriggerAttribute(1, 0, 0, 0));

            // ASSERT
            triggerBuilder.Should().NotBeNull();
            triggerBuilder.Should().BeOfType<SimpleTriggerAttributeBuilder>();
        }

        [Fact]
        public void TriggerBuilderTest_CronTriggerFromConfigAttributeBuilder()
        {
            // ACT
            var triggerBuilder = subject.GetTriggerBuilder(new CronTriggerFromConfigAttribute("trigger1"));

            // ASSERT
            triggerBuilder.Should().NotBeNull();
            triggerBuilder.Should().BeOfType<CronTriggerFromConfigAttributeBuilder>();
        }

        [Fact]
        public void TriggerBuilderTest_SimpleTriggerFromConfigAttributeBuilder()
        {
            // ACT
            var triggerBuilder = subject.GetTriggerBuilder(new SimpleTriggerFromConfigAttribute("trigger1"));

            // ASSERT
            triggerBuilder.Should().NotBeNull();
            triggerBuilder.Should().BeOfType<SimpleTriggerFromConfigAttributeBuilder>();
        }
    }
}
