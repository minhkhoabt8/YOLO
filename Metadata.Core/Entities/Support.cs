using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class Support
{
    public string SupportId { get; set; } = Guid.NewGuid().ToString();

    public string? SupportContent { get; set; }

    public string? SupportUnit { get; set; }

    public string? SupportNumber { get; set; }

    public decimal? SupportPrice { get; set; }

    public string? OwnerId { get; set; }

    public string? SupportTypeId { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual SupportType? SupportType { get; set; }
}
