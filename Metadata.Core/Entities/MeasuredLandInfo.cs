using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class MeasuredLandInfo
{
    public string MeasuredLandInfoId { get; set; } = null!;

    public string? MeasuredPageNumber { get; set; }

    public string? MeasuredPlotNumber { get; set; }

    public string? MeasuredPlotAddress { get; set; }

    public string? LandTypeId { get; set; }

    public string? MeasuredPlotArea { get; set; }

    public string? WidthdrawArea { get; set; }

    public string? GcnLandInfoId { get; set; }

    public string? OwnerId { get; set; }

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual GcnlandInfo? GcnLandInfo { get; set; }

    public virtual ICollection<LandCompensation> LandCompensations { get; } = new List<LandCompensation>();

    public virtual LandType? LandType { get; set; }
}
