using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;

namespace Metadata.Core.Entities;

public partial class ProjectDocument
{
    public string ProjectDocumentId { get; set; }

    public string? ProjectId { get; set; }

    public string? DocumentId { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Project? Project { get; set; }

    public static ProjectDocument CreateProjectDocument(string ProjectId, string DocumentId)
    {
        var projectDocument = new ProjectDocument
        {
           ProjectDocumentId = Guid.NewGuid().ToString(),
           ProjectId = ProjectId,
           DocumentId = DocumentId,
        };
        return projectDocument;
    }
}
