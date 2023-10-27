using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class ResettlementDocument
{
    public string ProjectDocumentId { get; set; } = null!;

    public string ResettlementProjectId { get; set; } = null!;

    public string DocumentId { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;

    public virtual ResettlementProject ResettlementProject { get; set; } = null!;


    public static ResettlementDocument CreateProjectDocument(string resettlementProjectId, string documentId)
    {
        var resettlementDocument = new ResettlementDocument
        {
            ProjectDocumentId = Guid.NewGuid().ToString(),
            ResettlementProjectId = resettlementProjectId,
            DocumentId = documentId,
        };
        return resettlementDocument;
    }
}
