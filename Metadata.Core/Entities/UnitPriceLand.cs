using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class UnitPriceLand
{
    public string UnitPriceLandId { get; set; } = null!;

    public string? ProjectId { get; set; }

    public string? StreetAreaName { get; set; }

    public string? LandTypeId { get; set; }

    public string? LandUnit { get; set; }

    public decimal? LandPosition1 { get; set; }

    public decimal? LandPosition2 { get; set; }

    public decimal? LandPosition3 { get; set; }

    public decimal? LandPosition4 { get; set; }

    public decimal? LandPosition5 { get; set; }

    public bool IsDeleted { get; set; }

    public virtual LandType? LandType { get; set; }

    public virtual ICollection<MeasuredLandInfo> MeasuredLandInfos { get; } = new List<MeasuredLandInfo>();

    public virtual Project? Project { get; set; }
}
