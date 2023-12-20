using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class Project : ITextSearchableEntity
{
    [Key]
    public string ProjectId { get; set; } = Guid.NewGuid().ToString();

    public string ProjectCode { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public string? ProjectLocation { get; set; }

    public string? Province { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public decimal? ProjectExpense { get; set; } = 0;

    public DateTime? ProjectApprovalDate { get; set; }

    public int? ImplementationYear { get; set; } = 2023;

    public string? RegulatedUnitPrice { get; set; }

    public int? ProjectBriefNumber { get; set; } = 0;

    public string? ProjectNote { get; set; }

    public string? PriceAppliedCodeId { get; set; }
    public string? ResettlementProjectId { get; set; }
    public string? CheckCode { get; set; }

    public string? ReportSignal { get; set; }

    public int? ReportNumber { get; set; } = 0;

    public string? PriceBasis { get; set; }

    public string? LandCompensationBasis { get; set; }

    public string? AssetCompensationBasis { get; set; }

    public DateTime ProjectCreatedTime { get; set; } = DateTime.UtcNow.SetKindUtc();

    public string ProjectCreatedBy { get; set; } = null!;

    public string ProjectStatus { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<LandPositionInfo>? LandPositionInfos { get; } = new List<LandPositionInfo>();

    public virtual ICollection<Owner>? Owners { get; } = new List<Owner>();

    public virtual ICollection<Plan>? Plans { get; } = new List<Plan>();

    public virtual PriceAppliedCode? PriceAppliedCode { get; set; }

    public virtual ICollection<ProjectDocument>? ProjectDocuments { get; } = new List<ProjectDocument>();

    public virtual ResettlementProject? ResettlementProject { get; set; }

    public virtual ICollection<UnitPriceLand>? UnitPriceLands { get; } = new List<UnitPriceLand>();

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(ProjectCode), .85},
        {() => nameof(ProjectName), .75}
    };
}
