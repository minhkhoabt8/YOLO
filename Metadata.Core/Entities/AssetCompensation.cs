using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class AssetCompensation
{
    [Key]
    public string AssetCompensationId { get; set; } = Guid.NewGuid().ToString();

    public string CompensationContent { get; set; } = null!;

    public int CompensationRate { get; set; } = 0;

    public int QuantityArea { get; set; } = 0;

    public decimal CompensationPrice { get; set; } = 0;

    public string CompensationType { get; set; } = null!;

    public string UnitPriceAssetId { get; set; } = null!;

    public string AssetUnitId { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    
    public virtual Owner Owner { get; set; } = null!;

    public virtual UnitPriceAsset UnitPriceAsset { get; set; } = null!;
    public virtual AssetUnit AssetUnit { get; set; } = null!;   
}
