using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class Deduction
{
    public string DeductionId { get; set; } = Guid.NewGuid().ToString();

    public string DeductionContent { get; set; } = null!;

    public decimal DeductionPrice { get; set; }

    public string? OwnerId { get; set; }

    public string? DeductionTypeId { get; set; }

    public virtual DeductionType? DeductionType { get; set; }

    public virtual Owner? Owner { get; set; }
}
