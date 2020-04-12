using FluentAssertions;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Configuration
{
    public partial class ConfigurationKeyBuilderTest
    {
        [Theory]
        [InlineData("Trigger1", (string)null, "Quartz.Triggers.Trigger1")]
        [InlineData("Trigger1", "TriggerGroup1", "Quartz.Triggers.TriggerGroup1.Trigger1")]
        public void BuildTriggerKey_NoParams(string triggerName, string triggerGroup, string expected)
        {
            // ARRANGE
            var triggerKey = new TriggerKey(triggerName, triggerGroup);

            // ACT
            var key = ConfigurationKeyBuilder.Build(triggerKey);

            // ASSERT
            key.Should().NotBeNull();
            key.Should().Be(expected);
        }

        [Theory]
        [InlineData("triggerGroup", "Quartz.Triggers.triggerGroup.trigger1.param1.param2")]
        [InlineData((string)null, "Quartz.Triggers.trigger1.param1.param2")]
        public void BuildTriggerKey_WithParams(string triggerGroup, string expectedKey)
        {
            // ARRANGE
            var triggerKey = new TriggerKey("trigger1", triggerGroup);

            // ACT
            var key = ConfigurationKeyBuilder.Build(triggerKey, "param1", "param2");

            // ASSERT
            key.Should().NotBeNull();
            key.Should().Be(expectedKey);
        }

        [Fact]
        public void BuildTriggerKey_NullTriggerKey()
        {
            // ARRANGE
            Action action = () => ConfigurationKeyBuilder.Build((TriggerKey)null);

            // ASSERT
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("triggerKey");
        }
    }
}
