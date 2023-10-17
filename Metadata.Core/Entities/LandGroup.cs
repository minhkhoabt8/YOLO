using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class LandGroup
{
    public string LandGroupId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<LandType> LandTypes { get; } = new List<LandType>();
}
