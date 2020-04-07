using FluentAssertions;
using Xunit;

namespace Quartz.AttributesExtension.Reflection
{
    public sealed class JobLocatorTests
    {
        [Fact]
        public void GetAllJobsInAppDomain_Test()
        {
            // ACT
            var jobs = JobLocator.GetAllJobsInAppDomain();

            // ASSERT
            jobs.Should().HaveCount(2);
            jobs.Should().ContainSingle(t => t.Name == typeof(SampleJob).Name);
            jobs.Should().ContainSingle(t => t.Name == typeof(SampleJob2).Name);
        }
    }
}
