using SharedLib.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

public partial class Plan : ITextSearchableEntity
{
    [Key]
    public string PlanId { get; set; } = Guid.NewGuid().ToString();

    public string? ProjectId { get; set; }

    public string? PlanCode { get; set; }

    public string? PlanPhrase { get; set; }

    public string? PlanDescription { get; set; }

    public string? PlanCreateBase { get; set; }

    public string? PlanApprovedBy { get; set; }

    public string? PlanReportSignal { get; set; }

    public DateTime? PlanReportDate { get; set; }

    public DateTime? PlanCreatedTime { get; set; }

    public DateTime? PlanEndedTime { get; set; }

    public string? PlanCreatedBy { get; set; }

    public string? PlanStatus { get; set; }

    //Tong Chu So Huu Ho Tro Boi Thuong
    public int? TotalOwnerSupportCompensation {  get; set; } = 0;

    //Tong Kinh Phi Boi Thuong
    public decimal? TotalPriceCompensation { get; set; } = 0;

    //Tong King Phi Boi Thuong Dat
    public decimal? TotalPriceLandSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceHouseSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceArchitectureSupportCompensation { get; set; } = 0;

    public decimal? TotalPricePlantSupportCompensation { get; set; } = 0;

    public decimal? TotalPriceOtherSupportCompensation { get; set; } = 0;

    //Tong Khau Tru
    public decimal? TotalDeduction { get; set; } = 0;

    public bool? IsDeleted { get; set; } = false;

    public virtual ICollection<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    public virtual ICollection<Owner> Owners { get; } = new List<Owner>();

    public virtual Project? Project { get; set; }

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => nameof(PlanCode), .5}
    };
}
