using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class AssetCompensation
{
    public string AssetCompensationId { get; set; } = Guid.NewGuid().ToString();

    public string CompensationContent { get; set; } = null!;

    public int CompensationRate { get; set; }

    public int QuantityArea { get; set; }

    public decimal CompensationPrice { get; set; }

    public string CompensationType { get; set; } = null!;

    public string UnitPriceAssetId { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual UnitPriceAsset UnitPriceAsset { get; set; } = null!;
}
