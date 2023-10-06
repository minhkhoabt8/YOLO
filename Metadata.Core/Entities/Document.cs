using Microsoft.AspNetCore.Http;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metadata.Core.Entities;

public partial class Document
{
    public string DocumentId { get; set; } = Guid.NewGuid().ToString();

    public string? DocumentTypeId { get; set; }

    public string Number { get; set; }

    public string Notation { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.Now.SetKindUtc();

    public DateTime PublishedDate { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string? Epitome { get; set; }

    public string? SignInfo { get; set; }

    public string? Note { get; set; }

    public string? Pen { get; set; }

    public string? ReferenceLink { get; set; }

    public bool? IsPublic { get; set; }

    public bool? IsDeleted { get; set; } = false;

    public virtual DocumentType? DocumentType { get; set; }

    public virtual ICollection<ProjectDocument> ProjectDocuments { get; } = new List<ProjectDocument>();
}
