using SharedLib.Core.Events;

namespace SharedLib.Infrastructure.EventHandlers;

public interface IEventDispatcher
{
    Task DispatchAsync(IEnumerable<IEvent> evts);
}