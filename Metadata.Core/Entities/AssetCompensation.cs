using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class AssetCompensation
{
    public string AssetCompensationId { get; set; } = null!;

    public string? CompensationContent { get; set; }

    public string? CompensationRate { get; set; }

    public int? QuantityArea { get; set; }

    public string? CompensationUnit { get; set; }

    public decimal? CompensationPrice { get; set; }

    public string? CompensationType { get; set; }

    public string? UnitPriceAssetId { get; set; }

    public string? OwnerId { get; set; }

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual Owner? Owner { get; set; }

    public virtual UnitPriceAsset? UnitPriceAsset { get; set; }
}
