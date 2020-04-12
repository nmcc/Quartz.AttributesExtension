using FluentAssertions;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Configuration
{
    public partial class ConfigurationKeyBuilderTest
    {
        [Theory]
        [InlineData("Job1", (string)null, "Quartz.Jobs.Job1")]
        [InlineData("Job1", "Group1", "Quartz.Jobs.Group1.Job1")]
        public void BuildJobKey_NoParams(string jobName, string jobGroup, string expected)
        {
            // ARRANGE
            var jobKey = new JobKey(jobName, jobGroup);

            // ACT
            var key = ConfigurationKeyBuilder.Build(jobKey);

            // ASSERT
            key.Should().NotBeNull();
            key.Should().Be(expected);
        }

        [Fact]
        public void BuildJobKey_NullJobKey()
        {
            // ARRANGE
            Action action = () => ConfigurationKeyBuilder.Build((JobKey)null);

            // ASSERT
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("jobKey");
        }

        [Theory]
        [InlineData("group1", "Quartz.Jobs.group1.job1.param1.param2")]
        [InlineData((string)null, "Quartz.Jobs.job1.param1.param2")]
        public void BuildJobKey_WithParams(string group, string expectedKey)
        {
            // ARRANGE
            var jobKey = new JobKey("job1", group);

            // ACT
            var key = ConfigurationKeyBuilder.Build(jobKey, "param1", "param2");

            // ASSERT
            key.Should().NotBeNull();
            key.Should().Be(expectedKey);
        }
    }
}
