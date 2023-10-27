using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class LandType
{
    public string LandTypeId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LandGroupId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<GcnlandInfo> GcnlandInfos { get; } = new List<GcnlandInfo>();

    public virtual LandGroup LandGroup { get; set; } = null!;

    public virtual ICollection<MeasuredLandInfo> MeasuredLandInfos { get; } = new List<MeasuredLandInfo>();

    public virtual ICollection<UnitPriceLand> UnitPriceLands { get; } = new List<UnitPriceLand>();
}
