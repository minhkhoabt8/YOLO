using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class SupportType
{
    public string SupportTypeId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Support> Supports { get; } = new List<Support>();
}
