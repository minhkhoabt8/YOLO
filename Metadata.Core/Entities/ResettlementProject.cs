using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class ResettlementProject
{
    public string ResettlementProjectId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int LimitToResettlement { get; set; }

    public int LimitToConsideration { get; set; }

    public string? Position { get; set; }

    public int LandNumber { get; set; }

    public string? ImplementYear { get; set; }

    public decimal LandPrice { get; set; }

    public string? Note { get; set; }

    public DateTime? LastDateEdit { get; set; }

    public string? LastPersonEdit { get; set; }

    public string DocumentId { get; set; } = null!;

    public string ProjectId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<LandResettlement> LandResettlements { get; } = new List<LandResettlement>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<ResettlementDocument> ResettlementDocuments { get; } = new List<ResettlementDocument>();
}
