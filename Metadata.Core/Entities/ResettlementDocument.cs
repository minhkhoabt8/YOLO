using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class ResettlementDocument
{
    public string ProjectDocumentId { get; set; } = null!;

    public string? ResettlementProjectId { get; set; }

    public string? DocumentId { get; set; }

    public virtual Document? Document { get; set; }

    public virtual ResettlementProject? ResettlementProject { get; set; }
}
