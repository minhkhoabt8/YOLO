using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class LandPositionInfo
{
    public string LandInfoPositionId { get; set; } = Guid.NewGuid().ToString();

    public string? LocationName { get; set; }

    public string? Description { get; set; }

    public string? ProjectId { get; set; }

    public virtual Project? Project { get; set; }
}
