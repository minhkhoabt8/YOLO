using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class ResettlementProject : ITextSearchableEntity
{
    [Key]
    public string ResettlementProjectId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal LimitToResettlement { get; set; } = 0;

    public decimal LimitToConsideration { get; set; } = 0;

    public string? Position { get; set; }

    public int LandNumber { get; set; } = 0;

    public string? ImplementYear { get; set; }

    public decimal LandPrice { get; set; } = 0;

    public string? Note { get; set; }

    public DateTime? LastDateEdit { get; set; } = DateTime.Now.SetKindUtc();

    public string? LastPersonEdit { get; set; }

    public string DocumentId { get; set; } = null!;

    public string? ProjectId { get; set; } 

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<LandResettlement> LandResettlements { get; } = new List<LandResettlement>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<ResettlementDocument> ResettlementDocuments { get; } = new List<ResettlementDocument>();

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(Code), .95},
        {() => nameof(Name), .85}
    };
}
