using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class AttachFile
{
    [Key]
    public string AttachFileId { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string ReferenceLink { get; set; } = null!;

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow.SetKindUtc();

    public string CreatedBy { get; set; } = null!;

    public string? PlanId { get; set; }

    public string? GcnLandInfoId { get; set; }

    public string? MeasuredLandInfoId { get; set; }

    public string? OwnerId { get; set; }

    public bool IsAssetCompensation { get; set; } = false;

    public virtual GcnlandInfo? GcnLandInfo { get; set; }

    public virtual MeasuredLandInfo? MeasuredLandInfo { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual Plan? Plan { get; set; }
}
