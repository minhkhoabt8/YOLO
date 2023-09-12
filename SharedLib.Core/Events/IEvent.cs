namespace SharedLib.Core.Events;

public interface IEvent
{
    public bool IsPublished { get; set; }
}