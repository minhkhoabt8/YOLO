using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class PriceAppliedCode
{
    public string PriceAppliedCodeId { get; set; } = Guid.NewGuid().ToString();

    public string UnitPriceCode { get; set; } = null!;

    public string PriceContent { get; set; } = null!;

    public DateTime ExpriredTime { get; set; }

    public virtual ICollection<Project> Projects { get; } = new List<Project>();

    public virtual ICollection<UnitPriceAsset> UnitPriceAssets { get; } = new List<UnitPriceAsset>();
}
