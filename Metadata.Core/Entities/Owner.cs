using SharedLib.Core.Entities;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class Owner : ITextSearchableEntity
{
    [Key]
    public string OwnerId { get; set; } = Guid.NewGuid().ToString();

    public string OwnerCode { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public string? OwnerIdCode { get; set; }

    public string? OwnerGender { get; set; }

    public DateTime? OwnerDateOfBirth { get; set; }

    public string? OwnerEthnic { get; set; }

    public string? OwnerNational { get; set; }

    public string? OwnerAddress { get; set; }

    public string? OwnerTaxCode { get; set; }

    public string? OwnerType { get; set; }

    public DateTime OwnerCreatedTime { get; set; } = DateTime.Now.SetKindUtc();

    public string OwnerCreatedBy { get; set; } = null!;

    public string? ProjectId { get; set; }

    public string? PlanId { get; set; }

    public string? OwnerStatus { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? PublishedPlace { get; set; }

    public string? HusbandWifeName { get; set; }

    public string? RepresentPerson { get; set; }

    public DateTime? TaxPublishedDate { get; set; }

    public string OrganizationTypeId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<AssetCompensation> AssetCompensations { get; } = new List<AssetCompensation>();

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual ICollection<Deduction> Deductions { get; } = new List<Deduction>();

    public virtual ICollection<GcnlandInfo> GcnlandInfos { get; } = new List<GcnlandInfo>();

    public virtual ICollection<LandResettlement> LandResettlements { get; } = new List<LandResettlement>();

    public virtual OrganizationType OrganizationType { get; set; } = null!;

    public virtual Plan? Plan { get; set; }

    public virtual Project? Project { get; set; }

    public virtual ICollection<Support> Supports { get; } = new List<Support>();

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(OwnerName), .5},
        {() => nameof(OwnerCode), .5},
        {() => nameof(OwnerIdCode), .5},
        {() => nameof(HusbandWifeName), .5},
        {() => nameof(OwnerTaxCode), .5},
        {() => nameof(RepresentPerson), .5},
        {() => nameof(OwnerType), .5}
    };
}
