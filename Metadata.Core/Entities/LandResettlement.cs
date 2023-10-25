using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class LandResettlement
{
    public string LandResettlementId { get; set; } = null!;

    public string? Position { get; set; }

    public string? PlotNumber { get; set; }

    public string? PageNumber { get; set; }

    public string? PlotAddress { get; set; }

    public decimal? LandSize { get; set; }

    public decimal? TotalLandPrice { get; set; }

    public string? ResettlementProjectId { get; set; }

    public string? OwnerId { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual ResettlementProject? ResettlementProject { get; set; }
}
