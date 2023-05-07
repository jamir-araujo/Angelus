using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Angelus.Subscriptions
{
    public class SubscriptionHostedService : IHostedService
    {
        private readonly IEnumerable<ISubscriptionSource> _sources;
        private readonly IMessageSubscriber _subscriber;

        public SubscriptionHostedService(IEnumerable<ISubscriptionSource> sources, IMessageSubscriber subscriber)
        {
            _sources = sources;
            _subscriber = subscriber;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var source in _sources)
            {
                source.Subscribe(_subscriber);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var source in _sources)
            {
                source.Unsubscribe();
            }

            return Task.CompletedTask;
        }
    }
}
