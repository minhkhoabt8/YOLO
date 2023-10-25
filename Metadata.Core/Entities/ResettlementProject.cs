using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class ResettlementProject
{
    public string ResettlementProjectId { get; set; } = null!;

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? LimitToResettlement { get; set; }

    public string? LimitToConsideration { get; set; }

    public string? Position { get; set; }

    public string? LandNumber { get; set; }

    public string? ImplementYear { get; set; }

    public decimal? LandPrice { get; set; }

    public string? Note { get; set; }

    public DateTime? LastDateEdit { get; set; }

    public string? LastPersonEdit { get; set; }

    public string? DocumentId { get; set; }

    public string? ProjectId { get; set; }

    public bool ? IsDeleted { get; set; } = false;

    public virtual ICollection<LandResettlement> LandResettlements { get; } = new List<LandResettlement>();

    public virtual Project? Project { get; set; }

    public virtual ICollection<ResettlementDocument> ResettlementDocuments { get; } = new List<ResettlementDocument>();
}
