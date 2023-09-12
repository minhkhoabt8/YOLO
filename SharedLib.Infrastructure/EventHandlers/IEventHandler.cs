using SharedLib.Core.Events;

namespace SharedLib.Infrastructure.EventHandlers;

public interface IEventHandler<in T> where T : IEvent
{
    Task HandleAsync(T evt);
}