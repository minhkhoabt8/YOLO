using System;
using System.Collections.Generic;

namespace Document.Core.Entities;

public partial class AuditTrail
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Type { get; set; }

    public string TableName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string NewValue { get; set; } = null!;

    public string? AffectedColumn { get; set; }

    public string PrimaryKey { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}
