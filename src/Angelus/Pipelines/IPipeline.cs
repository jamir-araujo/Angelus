using System;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Pipelines
{
    public interface IPipeline<in TContext>
    {
        Task SendAsync(TContext context, Func<Task> callback, CancellationToken cancellationToken = default);
    }
}
