using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class Document : ITextSearchableEntity
{
    [Key]
    public string DocumentId { get; set; } = Guid.NewGuid().ToString();

    public string DocumentTypeId { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Notation { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedTime { get; set; } = DateTime.Now.SetKindUtc();

    public DateTime PublishedDate { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string Epitome { get; set; } = null!;

    public string? SignInfo { get; set; }

    public string? Note { get; set; }

    public string? Pen { get; set; }

    public string FileName { get; set; } = null!;

    public int? FileSize { get; set; }

    public string ReferenceLink { get; set; } = null!;

    public bool IsUnitPriceLand { get; set; }

    public bool IsPublic { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual ICollection<ProjectDocument> ProjectDocuments { get; } = new List<ProjectDocument>();

    public virtual ICollection<ResettlementDocument> ResettlementDocuments { get; } = new List<ResettlementDocument>();

    public virtual ICollection<PriceAppliedCodeDocument> PriceAppliedCodeDocuments { get; } = new List<PriceAppliedCodeDocument>();

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(Number), .5},
        {() => nameof(Notation), .5},
        {() => nameof(DocumentTypeId), .5}
    };
}
