using SharedLib.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class PriceAppliedCode : ITextSearchableEntity
{
    [Key]
    public string PriceAppliedCodeId { get; set; } = Guid.NewGuid().ToString();

    public string UnitPriceCode { get; set; } = null!;

    public string PriceContent { get; set; } = null!;

    public DateTime ExpriredTime { get; set; }

    public virtual ICollection<Project> Projects { get; } = new List<Project>();

    public virtual ICollection<UnitPriceAsset> UnitPriceAssets { get; } = new List<UnitPriceAsset>();

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(UnitPriceCode), .100}
    };
}
