using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class AssetGroup
{
    public string AssetGroupId { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<UnitPriceAsset> UnitPriceAssets { get; } = new List<UnitPriceAsset>();
}
