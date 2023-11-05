using SharedLib.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class UnitPriceAsset : ITextSearchableEntity
{
    [Key]
    public string UnitPriceAssetId { get; set; } = Guid.NewGuid().ToString();

    public string AssetName { get; set; } = null!;

    public decimal AssetPrice { get; set; } = 0;

    public string? AssetRegulation { get; set; }

    public string AssetType { get; set; } = null!;

    public string PriceAppliedCodeId { get; set; } = null!;

    public string AssetUnitId { get; set; } = null!;

    public string AssetGroupId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AssetCompensation> AssetCompensations { get; } = new List<AssetCompensation>();

    public virtual AssetGroup AssetGroup { get; set; } = null!;

    public virtual AssetUnit AssetUnit { get; set; } = null!;

    public virtual PriceAppliedCode PriceAppliedCode { get; set; } = null!;

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(AssetName), .5},
        {() => nameof(AssetPrice), .5},
        {() => nameof(AssetRegulation),.5}
    };
}
