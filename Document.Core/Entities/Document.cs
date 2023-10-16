using System;
using System.Collections.Generic;

namespace Document.Core.Entities;

public partial class Document
{
    public string DocumentId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual ICollection<FileVersion> FileVersions { get; } = new List<FileVersion>();
}
