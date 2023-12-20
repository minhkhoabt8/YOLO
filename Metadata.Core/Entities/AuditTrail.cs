using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class AuditTrail : ITextSearchableEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Type { get; set; }

    public string TableName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? AffectedColumn { get; set; }

    public string PrimaryKey { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(UserId), .5},
        {() => nameof(UserName), .5}
    };
}
