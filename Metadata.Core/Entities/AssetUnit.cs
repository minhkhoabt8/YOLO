﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class AssetUnit
{
    [Key]
    public string AssetUnitId { get; set; } = Guid.NewGuid().ToString();

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<UnitPriceAsset> UnitPriceAssets { get; } = new List<UnitPriceAsset>();
    public virtual ICollection<Support> Supports { get; } = new List<Support>();
    public virtual ICollection<AssetCompensation> AssetCompensations { get; } = new List<AssetCompensation>();
}
