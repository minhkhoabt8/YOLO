﻿using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class Plan : ITextSearchableEntity
{
    public string PlanId { get; set; } = Guid.NewGuid().ToString();

    public string ProjectId { get; set; } = null!;

    public string PlanCode { get; set; } = null!;

    public string? PlanPhrase { get; set; }

    public string? PlanDescription { get; set; }

    public string? PlanCreateBase { get; set; }

    public string PlanApprovedBy { get; set; } = null!;

    public string? PlanReportSignal { get; set; }

    public DateTime? PlanReportDate { get; set; }

    public DateTime PlanCreatedTime { get; set; } = DateTime.Now.SetKindUtc();

    public DateTime PlanEndedTime { get; set; }

    public string PlanCreatedBy { get; set; } = null!;

    public string PlanStatus { get; set; } = null!;

    public int? TotalOwnerSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceCompensation { get; set; } = 0;

    public decimal? TotalPriceLandSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceHouseSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceArchitectureSupportCompensation { get; set; } = 0;

    public decimal? TotalPricePlantSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceOtherSupportCompensation { get; set; } = 0;

    public decimal? TotalDeduction { get; set; } = 0;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual ICollection<Owner> Owners { get; } = new List<Owner>();

    public virtual Project Project { get; set; } = null!;

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(PlanCode), .5}
    };
}
