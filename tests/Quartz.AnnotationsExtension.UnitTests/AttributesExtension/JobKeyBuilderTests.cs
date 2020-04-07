using FluentAssertions;
using Xunit;

namespace Quartz.AttributesExtension
{
    public sealed class JobKeyBuilderTests
    {
        [Theory]
        [InlineData("jobName", null, "jobName", "DEFAULT")]
        [InlineData("jobName", "jobGroup", "jobName", "jobGroup")]
        public void JobKeyBuilderTest(string jobAttribute_Name, string jobAttribute_Group, string expectedJobName, string expectedGroupName)
        {
            // ACT
            var jobKey = JobKeyBuilder.BuildJobKey(new JobAttribute(jobAttribute_Name, jobAttribute_Group), this.GetType());

            jobKey.Should().NotBeNull();
            jobKey.Name.Should().Be(expectedJobName);
            jobKey.Group.Should().Be(expectedGroupName);
        }

        [Fact]
        public void DefaultConstructor()
        {
            // ACT
            var jobKey = JobKeyBuilder.BuildJobKey(new JobAttribute(), this.GetType());

            jobKey.Should().NotBeNull();
            jobKey.Name.Should().Be(nameof(JobKeyBuilderTests));
            jobKey.Group.Should().Be("DEFAULT");
        }
    }
}
