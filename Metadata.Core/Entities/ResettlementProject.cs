using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class ResettlementProject
{
    [Key]
    public string ResettlementProjectId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int LimitToResettlement { get; set; } = 0;

    public int LimitToConsideration { get; set; } = 0;

    public string? Position { get; set; }

    public int LandNumber { get; set; } = 0;

    public string? ImplementYear { get; set; }

    public decimal LandPrice { get; set; } = 0;

    public string? Note { get; set; }

    public DateTime? LastDateEdit { get; set; } = DateTime.Now.SetKindUtc();

    public string? LastPersonEdit { get; set; }

    public string DocumentId { get; set; } = null!;

    public string ProjectId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<LandResettlement> LandResettlements { get; } = new List<LandResettlement>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<ResettlementDocument> ResettlementDocuments { get; } = new List<ResettlementDocument>();
}
