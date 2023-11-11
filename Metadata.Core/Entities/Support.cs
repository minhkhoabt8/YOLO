using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class Support
{
    [Key]
    public string SupportId { get; set; } = Guid.NewGuid().ToString();

    public string SupportContent { get; set; } = null!;

    public string AssetUnitId { get; set; } = null!;

    public decimal SupportUnitPrice { get; set; } = 0;

    public int SupportNumber { get; set; } = 0;

    public decimal SupportPrice { get; set; } = 0;

    public string OwnerId { get; set; } = null!;

    public string SupportTypeId { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual SupportType SupportType { get; set; } = null!;

    public virtual AssetUnit AssetUnit { get; set; } = null!;
}
