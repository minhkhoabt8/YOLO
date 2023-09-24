using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class DeductionType
{
    public string DeductionTypeId { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Deduction> Deductions { get; } = new List<Deduction>();
}
