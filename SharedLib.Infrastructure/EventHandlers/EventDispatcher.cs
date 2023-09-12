using Microsoft.Extensions.DependencyInjection;
using SharedLib.Core.Events;

namespace SharedLib.Infrastructure.EventHandlers;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventDispatcher(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceProvider = serviceProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task DispatchAsync(IEnumerable<IEvent> evts)
    {
        var evtsArr = evts as IEvent[] ?? evts.ToArray();
        var evtType = evtsArr.First().GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(evtType);
        var handleMethod = handlerType.GetMethod(nameof(IEventHandler<IEvent>.HandleAsync))!;

        // Intergration event should use a seperate scope because it may be processed after the initial request ended
        if (evtType.IsAssignableTo(typeof(IntegrationEvent)))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService(handlerType);

            foreach (var evt in evtsArr)
            {
                await (handleMethod.Invoke(handler, new object[] {evt}) as Task)!;
                evt.IsPublished = true;
            }
        }
        // Handle domain events using same request scope
        else
        {
            var handler = _serviceProvider.GetService(handlerType);

            foreach (var evt in evtsArr)
            {
                await (handleMethod.Invoke(handler, new object[] {evt}) as Task)!;
                evt.IsPublished = true;
            }
        }
    }
}