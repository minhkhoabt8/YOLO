using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class PriceAppliedCode
{
    public string PriceAppliedCodeId { get; set; } = null!;

    public string? UnitPriceCode { get; set; }

    public string? PriceContent { get; set; }

    public DateTime? ExpriredTime { get; set; }

    public virtual ICollection<Project> Projects { get; } = new List<Project>();

    public virtual ICollection<UnitPriceAsset> UnitPriceAssets { get; } = new List<UnitPriceAsset>();
}
