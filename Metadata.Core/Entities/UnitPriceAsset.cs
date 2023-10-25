using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class UnitPriceAsset
{
    public string UnitPriceAssetId { get; set; } = null!;

    public string? AssetName { get; set; }

    public decimal? AssetPrice { get; set; }

    public string? AssetRegulation { get; set; }

    public string? AssetType { get; set; }

    public string? PriceAppliedCodeId { get; set; }

    public string? AssetUnitId { get; set; }

    public string? AssetGroupId { get; set; }

    public bool IsDeleted {  get; set; } = false;

    public virtual ICollection<AssetCompensation> AssetCompensations { get; } = new List<AssetCompensation>();

    public virtual AssetGroup? AssetGroup { get; set; }

    public virtual AssetUnit? AssetUnit { get; set; }

    public virtual PriceAppliedCode? PriceAppliedCode { get; set; }
}
