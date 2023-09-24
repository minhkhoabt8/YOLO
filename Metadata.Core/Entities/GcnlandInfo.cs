using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class GcnlandInfo
{
    public string GcnLandInfoId { get; set; } = null!;

    public string? GcnPageNumber { get; set; }

    public string? GcnPlotNumber { get; set; }

    public string? GcnPlotAddress { get; set; }

    public string? LandTypeId { get; set; }

    public string? GcnPlotArea { get; set; }

    public string? GcnOwnerCertificate { get; set; }

    public string? OwnerId { get; set; }

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual LandType? LandType { get; set; }

    public virtual ICollection<MeasuredLandInfo> MeasuredLandInfos { get; } = new List<MeasuredLandInfo>();

    public virtual Owner? Owner { get; set; }
}
