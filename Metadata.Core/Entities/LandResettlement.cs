using SharedLib.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class LandResettlement : ITextSearchableEntity
{
    [Key]
    public string LandResettlementId { get; set; } = Guid.NewGuid().ToString();

    public string? Position { get; set; }

    public string? PlotNumber { get; set; }

    public string? PageNumber { get; set; }

    public string? PlotAddress { get; set; }

    public decimal? LandSize { get; set; } = 0;

    public decimal? TotalLandPrice { get; set; } = 0;

    public string? ResettlementProjectId { get; set; } 

    public string? OwnerId { get; set; } 

    public virtual Owner? Owner { get; set; } 

    public virtual ResettlementProject? ResettlementProject { get; set; } = null!;

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(Position), .95}
    };
}
