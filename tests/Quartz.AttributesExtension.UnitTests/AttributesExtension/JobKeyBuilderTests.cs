using FluentAssertions;
using Xunit;

namespace Quartz.AttributesExtension
{
    public class JobKeyBuilderTests
    {
        [Fact]
        public void DefaultCtor()
        {
            // ARRANGE
            var jobAttr = new JobAttribute();

            // ACT
            var key = JobKeyBuilder.BuildJobKey(jobAttr, typeof(SampleJob));

            // ASSERT
            key.Should().NotBeNull();
            key.Name.Should().Be(nameof(SampleJob));
            key.Group.Should().Be("DEFAULT");
        }

        [Theory]
        [InlineData("MyJob", "MyGroup", "MyJob", "MyGroup")]
        [InlineData("MyJob", (string)null, "MyJob", "DEFAULT")]
        public void NameAndGroup(string name, string group, string expectedJobName, string expectedGroupName)
        {
            // ARRANGE
            var jobAttr = new JobAttribute(name, group);

            // ACT
            var key = JobKeyBuilder.BuildJobKey(jobAttr, typeof(SampleJob));

            // ASSERT
            key.Should().NotBeNull();
            key.Name.Should().Be(expectedJobName);
            key.Group.Should().Be(expectedGroupName);
        }
    }
}
