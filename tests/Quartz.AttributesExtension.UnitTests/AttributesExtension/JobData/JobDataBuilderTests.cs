using FluentAssertions;
using Moq;
using Quartz.AttributesExtension.Configuration;
using System;
using Xunit;

namespace Quartz.AttributesExtension.JobData
{
    public sealed class JobDataBuilderTests : IDisposable
    {
        private readonly JobDataBuilder subject;
        private readonly Mock<IConfigurationProvider> configurationProviderMock;

        public JobDataBuilderTests()
        {
            this.configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
            this.subject = new JobDataBuilder(configurationProviderMock.Object);
        }

        public void Dispose()
        {
            this.configurationProviderMock.VerifyAll();
        }

        [Fact]
        public void HappyPath()
        {
            // ARRANGE
            this.configurationProviderMock.Setup(m => m.GetString("Quartz.Jobs.SampleJob.BoolParam")).Returns("true");
            this.configurationProviderMock.Setup(m => m.GetString("Quartz.Jobs.SampleJob.IntParam")).Returns("10");

            // ACT
            var jobDataMap = this.subject.Build(typeof(SampleJob), new JobKey(nameof(SampleJob)));

            // ASSERT
            jobDataMap.Should().NotBeNull();
            jobDataMap.GetString(nameof(SampleJob.Param1)).Should().Be("Lorem");
            jobDataMap.GetInt(nameof(SampleJob.IntParam)).Should().Be(10);
            jobDataMap.GetBoolean(nameof(SampleJob.BoolParam)).Should().Be(true);
        }
    }
}
