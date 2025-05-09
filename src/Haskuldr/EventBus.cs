using System.Reflection;
using Haskuldr.Abstractions;
using Haskuldr.Abstractions.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Haskuldr;

public class EventBus(IServiceProvider serviceProvider) : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        var handlers = serviceProvider
                       .GetServices<IEventHandler<TEvent>>()
                       .OrderBy(x =>
                       {
                           var orderAttribute = x.GetType()
                                                 .GetCustomAttribute<OrderAttribute>();

                           return orderAttribute?.Order ?? int.MaxValue;
                       });

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}