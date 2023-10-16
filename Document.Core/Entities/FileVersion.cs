using System;
using System.Collections.Generic;

namespace Document.Core.Entities;

public partial class FileVersion
{
    public string VersionId { get; set; } = Guid.NewGuid().ToString();

    public string DocumentId { get; set; } = null!;

    public double VersionNumber { get; set; }

    public string? ChangeDescription { get; set; }

    public DateTime CreatedTime { get; set; }

    public string ReferenceLink { get; set; } = null!;

    public bool IsReady { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Document Document { get; set; } = null!;
}
