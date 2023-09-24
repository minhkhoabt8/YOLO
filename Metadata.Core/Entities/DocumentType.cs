using System;
using System.Collections.Generic;

namespace Metadata.Core.Entities;

public partial class DocumentType
{
    public string DocumentTypeId { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; } = new List<Document>();
}
