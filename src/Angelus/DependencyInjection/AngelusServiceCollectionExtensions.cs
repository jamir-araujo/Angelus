using System;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AngelusServiceCollectionExtensions
    {
        private static readonly ConcurrentDictionary<IServiceCollection, IAngelusBuilder> _builders =
            new ConcurrentDictionary<IServiceCollection, IAngelusBuilder>();

        public static IServiceCollection AddAngelus(this IServiceCollection services, Action<IAngelusBuilder> configure)
        {
            var builder = _builders.GetOrAdd(services, s => new AngelusBuilder(s));

            configure(builder);

            return services;
        }
    }
}
