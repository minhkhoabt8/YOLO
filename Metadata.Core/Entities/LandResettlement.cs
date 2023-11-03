using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class LandResettlement
{
    [Key]
    public string LandResettlementId { get; set; } = Guid.NewGuid().ToString();

    public string? Position { get; set; }

    public string? PlotNumber { get; set; }

    public string? PageNumber { get; set; }

    public string? PlotAddress { get; set; }

    public decimal? LandSize { get; set; } = 0;

    public decimal? TotalLandPrice { get; set; } = 0;

    public string ResettlementProjectId { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual ResettlementProject ResettlementProject { get; set; } = null!;
}
