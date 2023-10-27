using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class Support
{
    public string SupportId { get; set; } = Guid.NewGuid().ToString();

    public string SupportContent { get; set; } = null!;

    public string? SupportUnit { get; set; }

    public int SupportNumber { get; set; }

    public decimal SupportPrice { get; set; }

    public string OwnerId { get; set; } = null!;

    public string SupportTypeId { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual SupportType SupportType { get; set; } = null!;
}
