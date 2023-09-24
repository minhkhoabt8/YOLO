using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class ProjectDocument
{
    public string ProjectDocumentId { get; set; } = null!;

    public string? ProjectId { get; set; }

    public string? DocumentId { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Project? Project { get; set; }
}
