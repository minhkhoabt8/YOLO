using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class ProjectDocument
{
    public string ProjectDocumentId { get; set; } = null!;

    public string ProjectId { get; set; } = null!;

    public string DocumentId { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

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
