using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Quartz.AttributesExtension.Trigger
{
    public sealed class TriggerKeyBuilderTests : IDisposable
    {
        private readonly Mock<ITriggerAttribute> triggerMock = new Mock<ITriggerAttribute>(MockBehavior.Strict);

        public void Dispose()
        {
            this.triggerMock.VerifyAll();
        }

        [Theory]
        [InlineData((string)null, "Trigger1", "DEFAULT", "Trigger1")]
        [InlineData("Group1", "Trigger1", "Group1", "Trigger1")]
        public void NameAndGroup(string group, string name, string expectedGroup, string expectedName)
        {
            // ARRANGE
            triggerMock.Setup(t => t.Group).Returns(group);
            triggerMock.Setup(t => t.Name).Returns(name);

            // ACT
            var triggerKey = TriggerKeyBuilder.BuildTriggerKey(triggerMock.Object);

            // ASSERT
            triggerKey.Should().NotBeNull();
            triggerKey.Group.Should().Be(expectedGroup);
            triggerKey.Name.Should().Be(expectedName);
        }

        [Fact]
        public void NoName()
        {
            // ARRANGE
            triggerMock.Setup(t => t.Name).Returns<string>(null);

            // ACT
            var triggerKey = TriggerKeyBuilder.BuildTriggerKey(triggerMock.Object);

            // ASSERT
            triggerKey.Should().NotBeNull();
            triggerKey.Group.Should().Be("DEFAULT");
            triggerKey.Name.Should().NotBeNullOrEmpty();
            triggerKey.Name.Should().MatchRegex("^[a-f0-9-]+$");
        }
    }
}
