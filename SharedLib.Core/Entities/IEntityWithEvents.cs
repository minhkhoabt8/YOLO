using System.ComponentModel.DataAnnotations.Schema;
using SharedLib.Core.Events;

namespace SharedLib.Core.Entities;

public interface IEntityWithEvents
{
    [NotMapped] public List<IEvent> Events { get; set; }
}