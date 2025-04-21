using System.Reflection;
using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus;

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