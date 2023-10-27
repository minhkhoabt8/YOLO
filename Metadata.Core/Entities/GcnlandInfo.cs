using SharedLib.Core.Entities;
using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class GcnlandInfo : ITextSearchableEntity
{
    public string GcnLandInfoId { get; set; } = Guid.NewGuid().ToString();

    public string GcnPageNumber { get; set; } = null!;

    public string GcnPlotNumber { get; set; } = null!;

    public string GcnPlotAddress { get; set; } = null!;

    public string LandTypeId { get; set; } = null!;

    public decimal? GcnPlotArea { get; set; }

    public string? GcnOwnerCertificate { get; set; }

    public string OwnerId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual LandType LandType { get; set; } = null!;

    public virtual ICollection<MeasuredLandInfo> MeasuredLandInfos { get; } = new List<MeasuredLandInfo>();

    public virtual Owner Owner { get; set; } = null!;

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(GcnPageNumber), .5},
        {() => nameof(GcnPlotNumber), .5},
        {() => nameof(GcnPlotAddress), .5},
        {() => nameof(GcnOwnerCertificate), .5}
    };
}
