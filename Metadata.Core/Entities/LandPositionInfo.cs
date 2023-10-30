using SharedLib.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class LandPositionInfo : ITextSearchableEntity
{
    [Key]
    public string LandInfoPositionId { get; set; } = Guid.NewGuid().ToString();
    public string LocationName { get; set; } = null!;

    public string? Description { get; set; }

    public string ProjectId { get; set; } = null!;

    public string LandInfoType { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(LocationName), .5}
    };
}
