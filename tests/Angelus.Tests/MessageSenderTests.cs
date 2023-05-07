using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Angelus.Sending;
using Angelus.Transport;

using Moq;

using Xunit;

namespace Angelus.Tests
{
    public class MessageSenderTests
    {
        private readonly Mock<ISendContextBuilder> _mockBuilder;
        private readonly List<ITransportSender> _senders;
        private readonly MessageSender _sender;

        public MessageSenderTests()
        {
            _mockBuilder = new Mock<ISendContextBuilder>();
            _senders = new List<ITransportSender>();

            _sender = new MessageSender(_mockBuilder.Object, _senders);
        }

        [Fact]
        public void Constructor_Should_Throw_When_builderIsNull()
        {
            Assert.Throws<ArgumentNullException>("builder", () => new MessageSender(null, new List<ITransportSender>()));
        }

        [Fact]
        public void Constructor_Should_Throw_When_sendersIsNull()
        {
            Assert.Throws<ArgumentNullException>("senders", () => new MessageSender(_mockBuilder.Object, null));
        }

        [Fact]
        public async Task SendAsync_Should_CallTheContextBuilder_When_NotNull()
        {
            var contextBuilderCalled = false;

            await _sender.SendAsync(new object(), c =>
            {
                contextBuilderCalled = true;
                return Task.CompletedTask;
            });

            Assert.True(contextBuilderCalled);
        }

        [Fact]
        public async Task SendAsync_Should_BuildOnTheBuilder()
        {
            await _sender.SendAsync(new object(), null);

            _mockBuilder.Verify(b => b.Build(It.IsAny<ISendContext<object>>()), Times.Once);
        }

        [Fact]
        public async Task SendAsync_Should_CallAllTransportSenders()
        {
            var mockSender = new Mock<ITransportSender>();

            _senders.Add(mockSender.Object);
            _senders.Add(mockSender.Object);
            _senders.Add(mockSender.Object);
            _senders.Add(mockSender.Object);

            await _sender.SendAsync(new object(), null);

            mockSender.Verify(
                s => s.SendAsync(It.IsAny<ISendContext<object>>(), CancellationToken.None),
                Times.Exactly(_senders.Count));
        }

        [Fact]
        public async Task SendAsync_Should_PassTheCancellationTokenToTheTransportSenders()
        {
            var mockSender = new Mock<ITransportSender>();

            _senders.Add(mockSender.Object);
            _senders.Add(mockSender.Object);
            _senders.Add(mockSender.Object);
            _senders.Add(mockSender.Object);

            var source = new CancellationTokenSource();
            var token = source.Token;

            await _sender.SendAsync(new object(), null, token);

            mockSender.Verify(
                s => s.SendAsync(It.IsAny<ISendContext<object>>(), token),
                Times.Exactly(_senders.Count));
        }
    }
}
