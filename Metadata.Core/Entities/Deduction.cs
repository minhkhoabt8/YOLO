using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class Deduction
{
    [Key]
    public string DeductionId { get; set; } = Guid.NewGuid().ToString();

    public string DeductionContent { get; set; } = null!;

    public decimal DeductionPrice { get; set; } = 0;

    public string? OwnerId { get; set; }

    public string? DeductionTypeId { get; set; }

    public virtual DeductionType? DeductionType { get; set; }

    public virtual Owner? Owner { get; set; }
}
