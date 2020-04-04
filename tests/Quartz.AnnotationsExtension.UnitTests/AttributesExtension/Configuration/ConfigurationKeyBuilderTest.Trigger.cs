using FluentAssertions;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Configuration
{
    public partial class ConfigurationKeyBuilderTest
    {
        [Theory]
        [InlineData("Job1", (string)null, "Trigger1", (string)null, "Jobs.Job1.Trigger1")]
        [InlineData("Job1", "Group1", "Trigger1", (string)null, "Jobs.Group1.Job1.Trigger1")]
        [InlineData("Job1", "JobGroup1", "Trigger1", "TriggerGroup1", "Jobs.JobGroup1.Job1.TriggerGroup1.Trigger1")]
        public void BuildTriggerKey_NoParams(string jobName, string jobGroup, string triggerName, string triggerGroup, string expected)
        {
            // ARRANGE
            var jobKey = new JobKey(jobName, jobGroup);
            var triggerKey = new TriggerKey(triggerName, triggerGroup);

            // ACT
            var key = ConfigurationKeyBuilder.Build(jobKey, triggerKey);

            // ASSERT
            key.Should().NotBeNull();
            key.Should().Be(expected);
        }

        [Fact]
        public void BuildTriggerKey_WithParams()
        {
            // ARRANGE
            var jobKey = new JobKey("job1");
            var triggerKey = new TriggerKey("trigger1");

            // ACT
            var key = ConfigurationKeyBuilder.Build(jobKey, triggerKey, "param1", "param2");

            // ASSERT
            key.Should().NotBeNull();
            key.Should().Be("Jobs.job1.trigger1.param1.param2");
        }

        [Fact]
        public void BuildTriggerKey_NullJobKey()
        {
            // ARRANGE
            Action action = () => ConfigurationKeyBuilder.Build(null, new TriggerKey("trigger1"));

            // ASSERT
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("jobKey");
        }

        [Fact]
        public void BuildTriggerKey_NullTriggerKey()
        {
            // ARRANGE
            Action action = () => ConfigurationKeyBuilder.Build(new JobKey("job1"), (TriggerKey)null);

            // ASSERT
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("triggerKey");
        }
    }
}
