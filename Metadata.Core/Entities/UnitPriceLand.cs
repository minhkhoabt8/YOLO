using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class UnitPriceLand
{
    [Key]
    public string UnitPriceLandId { get; set; } = Guid.NewGuid().ToString();

    public string ProjectId { get; set; } = null!;

    public string StreetAreaName { get; set; } = null!;

    public string LandTypeId { get; set; } = null!;

    public string LandUnit { get; set; } = null!;

    public decimal? LandPosition1 { get; set; }

    public decimal? LandPosition2 { get; set; }

    public decimal? LandPosition3 { get; set; }

    public decimal? LandPosition4 { get; set; }

    public decimal? LandPosition5 { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual LandType LandType { get; set; } = null!;

    public virtual ICollection<MeasuredLandInfo> MeasuredLandInfos { get; } = new List<MeasuredLandInfo>();

    public virtual Project Project { get; set; } = null!;
}
