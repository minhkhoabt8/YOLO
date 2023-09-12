namespace SharedLib.Core.Events;

public class DomainEvent : IEvent
{
    public bool IsPublished { get; set; } = false;
}