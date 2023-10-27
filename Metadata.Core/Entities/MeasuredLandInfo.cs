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

    public string LandTypeId { get; set; } = null!;

    public decimal? MeasuredPlotArea { get; set; }

    public decimal? WithdrawArea { get; set; }

    public decimal? CompensationPrice { get; set; }

    public decimal? CompensationRate { get; set; }

    public string? CompensationNote { get; set; }

    public string GcnLandInfoId { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public string UnitPriceLandId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual GcnlandInfo GcnLandInfo { get; set; } = null!;

    public virtual LandType LandType { get; set; } = null!;

    public virtual UnitPriceLand UnitPriceLand { get; set; } = null!;
    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(MeasuredPageNumber), .5},
        {() => nameof(MeasuredPlotNumber), .5},
        {() => nameof(MeasuredPlotArea), .5}
    };
}
