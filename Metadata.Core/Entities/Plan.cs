using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class Plan
{
    public string PlanId { get; set; } = null!;

    public string? ProjectId { get; set; }

    public string? PlaneCode { get; set; }

    public string? PlanPhrase { get; set; }

    public string? PlanDescription { get; set; }

    public string? PlanCreateBase { get; set; }

    public string? PlanApprovedBy { get; set; }

    public string? PlanReportSignal { get; set; }

    public DateTime? PlanReportDate { get; set; }

    public DateTime? PlanCreatedTime { get; set; }

    public DateTime? PlanEndedTime { get; set; }

    public string? PlanCreatedBy { get; set; }

    public bool? PlanStatus { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual ICollection<Owner> Owners { get; } = new List<Owner>();

    public virtual Project? Project { get; set; }
}
