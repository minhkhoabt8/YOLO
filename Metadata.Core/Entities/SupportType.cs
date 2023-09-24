using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class SupportType
{
    public string SupportTypeId { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Support> Supports { get; } = new List<Support>();
}
