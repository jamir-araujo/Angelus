using System;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Pipelines
{
    public delegate Task Middleware<in TContext>(TContext context, Func<Task> next, CancellationToken cancellationToken = default);

    public interface IMiddleware<in TContext>
    {
        Task InvokeAsync(TContext context, Func<Task> next, CancellationToken cancellationToken = default);
    }
}
