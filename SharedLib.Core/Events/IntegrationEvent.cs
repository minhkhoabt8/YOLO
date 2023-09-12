namespace SharedLib.Core.Events;

public class IntegrationEvent : IEvent
{
    public bool IsPublished { get; set; } = false;
}