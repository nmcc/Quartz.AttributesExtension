using FluentAssertions;
using Moq;
using Quartz.AttributesExtension.Configuration;
using Xunit;

namespace Quartz.AttributesExtension.JobData
{
    public sealed class JobDataBuilderTests
    {
        private readonly JobDataBuilder subject;
        private readonly Mock<IConfigurationProvider> configurationProviderMock;

        public JobDataBuilderTests()
        {
            this.configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            this.subject = new JobDataBuilder(configurationProviderMock.Object);
        }

        [Fact]
        public void HappyPath()
        {
            // ARRANGE
            this.configurationProviderMock.Setup(m => m.GetString("Jobs.SampleJob.BoolParam")).Returns("true");
            this.configurationProviderMock.Setup(m => m.GetString("Jobs.SampleJob.IntParam")).Returns("10");

            // ACT
            var jobDataMap = this.subject.Build<SampleJob>(new JobKey(nameof(SampleJob)));

            // ASSERT
            jobDataMap.Should().NotBeNull();
            jobDataMap.GetString(nameof(SampleJob.Param1)).Should().Be("Lorem");
            jobDataMap.GetInt(nameof(SampleJob.IntParam)).Should().Be(10);
            jobDataMap.GetBoolean(nameof(SampleJob.BoolParam)).Should().Be(true);

            this.configurationProviderMock.VerifyAll();
        }
    }
}
