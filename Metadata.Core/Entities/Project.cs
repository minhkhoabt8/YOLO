using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class Project : ITextSearchableEntity
{
    public string ProjectId { get; set; } = Guid.NewGuid().ToString();

    public string ProjectCode { get; set; }

    public string ProjectName { get; set; } 

    public string? ProjectLocation { get; set; }

    public string? Province { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public decimal? ProjectExpense { get; set; }

    public DateTime? ProjectApprovalDate { get; set; }

    public string? ImplementationYear { get; set; }

    public string? RegulatedUnitPrice { get; set; }

    public string? ProjectBriefNumber { get; set; }

    public string? ProjectNote { get; set; }

    public string? PriceAppliedCodeId { get; set; }

    public string? CheckCode { get; set; }

    public string? ReportSignal { get; set; }

    public string? ReportNumber { get; set; }

    public string? PriceBasis { get; set; }

    public string? LandCompensationBasis { get; set; }

    public string? AssetCompensationBasis { get; set; }

    public DateTime? ProjectCreatedTime { get; set; } = DateTime.Now.SetKindUtc();

    public string? ProjectCreatedBy { get; set; }

    public bool? ProjectStatus { get; set; }

    public bool? IsDeleted { get; set; } = false;

    public virtual ICollection<LandPositionInfo> LandPositionInfos { get; } = new List<LandPositionInfo>();

    public virtual ICollection<Owner> Owners { get; } = new List<Owner>();

    public virtual ICollection<Plan> Plans { get; } = new List<Plan>();

    public virtual PriceAppliedCode? PriceAppliedCode { get; set; }

    public virtual ICollection<ProjectDocument> ProjectDocuments { get; } = new List<ProjectDocument>();

    public virtual ICollection<UnitPriceLand> UnitPriceLands { get; } = new List<UnitPriceLand>();

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(ProjectCode), .5},
        {() => nameof(ProjectName), .5}
    };
}
