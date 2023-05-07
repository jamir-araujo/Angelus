
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Angelus.Tests.DependencyInjection
{
    public class TransportBuilderTests
    {
        [Fact]
        public void GetOrAddTransport_Should_ReturnANewInstance()
        {
            var builder = new AngelusBuilder(new ServiceCollection());

            var transportBuilder = new TransportBuilderABC();
            var returnedBuilder = builder.GetOrAddTransport("transport", name => transportBuilder);

            Assert.Same(transportBuilder, returnedBuilder);
        }

        [Fact]
        public void GetOrAddTransport_Should_ReturnTheSameInstance_When_TransportNameIsTheSame()
        {
            var builder = new AngelusBuilder(new ServiceCollection());

            var result1 = builder.GetOrAddTransport("transport1", name => new TransportBuilderABC());
            var result2 = builder.GetOrAddTransport("transport1", name => new TransportBuilderABC());

            Assert.Same(result1, result2);
        }

        [Fact]
        public void GetOrAddTransport_Should_ReturnTheADiffentInstance_When_TransportNameIsNotTheSame()
        {
            var builder = new AngelusBuilder(new ServiceCollection());

            var result1 = builder.GetOrAddTransport("transport1", name => new TransportBuilderABC());
            var result2 = builder.GetOrAddTransport("transport2", name => new TransportBuilderABC());

            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void GetOrAddTransport_Should_ReturnTheADiffentInstance_When_TransportNameIsTheSameButTypeIsDifferent()
        {
            var builder = new AngelusBuilder(new ServiceCollection());

            var result1 = builder.GetOrAddTransport("transport1", name => new TransportBuilderABC());
            var result2 = builder.GetOrAddTransport("transport1", name => new TransportBuilderXYZ());

            Assert.NotEqual((object)result1, (object)result2);
        }

        class TransportBuilderABC
        {
        }

        class TransportBuilderXYZ
        {
        }
    }
}
