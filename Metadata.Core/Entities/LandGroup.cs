using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class LandGroup
{
    public string LandGroupId { get; set; } = null!;

    public string? Code { get; set; }

    public string? Name { get; set; }



    public virtual ICollection<LandType> LandTypes { get; } = new List<LandType>();
}
