using SharedLib.Core.Entities;
using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class MeasuredLandInfo : ITextSearchableEntity
{
    public string MeasuredLandInfoId { get; set; } = Guid.NewGuid().ToString(); 

    public string? MeasuredPageNumber { get; set; }

    public string? MeasuredPlotNumber { get; set; }

    public string? MeasuredPlotAddress { get; set; }

    public string? LandTypeId { get; set; }

    public string? MeasuredPlotArea { get; set; }

    public decimal? WidthdrawArea { get; set; } = 0;

    public decimal? CompensationPrice { get; set; } = 0;

    public decimal? CompensationRate { get; set; } = 0;

    public string? CompensationNote { get; set; }

    public string? GcnLandInfoId { get; set; }

    public string? OwnerId { get; set; }

    public string? UnitPriceLandId { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual GcnlandInfo? GcnLandInfo { get; set; }

    public virtual LandType? LandType { get; set; }

    public virtual UnitPriceLand? UnitPriceLand { get; set; }

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(MeasuredPageNumber), .5},
        {() => nameof(MeasuredPlotNumber), .5},
        {() => nameof(MeasuredPlotArea), .5}
    };
}
