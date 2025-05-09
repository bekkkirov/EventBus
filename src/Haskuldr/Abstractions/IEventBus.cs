using Haskuldr.Abstractions.EventBus;

namespace Haskuldr.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
}