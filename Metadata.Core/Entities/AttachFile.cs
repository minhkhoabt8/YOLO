using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class AttachFile
{
    public string AttachFileId { get; set; } = null!;

    public string? Name { get; set; }

    public string? FileType { get; set; }

    public string? ReferenceLink { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? PlanId { get; set; }

    public string? GcnLandInfoId { get; set; }

    public string? MeasuredLandInfoId { get; set; }

    public string? OwnerId { get; set; }

    public string? AssetCompensationId { get; set; }

    public virtual AssetCompensation? AssetCompensation { get; set; }

    public virtual GcnlandInfo? GcnLandInfo { get; set; }

    public virtual MeasuredLandInfo? MeasuredLandInfo { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual Plan? Plan { get; set; }
}
