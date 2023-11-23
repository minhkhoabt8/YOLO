using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.DTOs.UnitPriceLand;


namespace Metadata.Infrastructure.DTOs.Project
{
    public class ProjectReadDTO
    {
        public string? ProjectId { get; set; } 

        public string? ProjectCode { get; set; }

        public string? ProjectName { get; set; }

        public string? ProjectLocation { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public decimal? ProjectExpense { get; set; }

        public DateTime? ProjectApprovalDate { get; set; }

        public int? ImplementationYear { get; set; }

        public string? RegulatedUnitPrice { get; set; }

        public int? ProjectBriefNumber { get; set; }

        public string? ProjectNote { get; set; }

        public string? PriceAppliedCodeId { get; set; }

        public string? ResettlementProjectId { get; set; }

        public string? CheckCode { get; set; }

        public string ReportSignal { get; set; }

        public int? ReportNumber { get; set; }

        public string? PriceBasis { get; set; }

        public string? LandCompensationBasis { get; set; }

        public string? AssetCompensationBasis { get; set; }

        public DateTime? ProjectCreatedTime { get; set; }

        public string? ProjectCreatedBy { get; set; }

        public string? ProjectStatus { get; set; }

        public PriceAppliedCodeReadDTO? PriceAppliedCode { get; set; }

        public IEnumerable<LandPositionInfoReadDTO>? LandPositionInfos { get; set; }

        public IEnumerable<OwnersInProjectDTO>? Owners {  get; set; }

        public IEnumerable<PlansInProjectDTO>? Plans { get; set; }

        public ResettlementProjectReadDTO? ResettlementProject { get; set; }
        //Attach Documents manual
        public IEnumerable<DocumentReadDTO>? ProjectDocuments { get; set; }

        public IEnumerable<UnitPriceLandReadDTO>? UnitPriceLands { get; set; }

    }

    public class OwnersInProjectDTO
    {
        public string OwnerId { get; set; } = null!;

        public string? OwnerCode { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerIdCode { get; set; }

        public string? OwnerGender { get; set; }

        public DateTime? OwnerDateOfBirth { get; set; }

        public string? OwnerEthnic { get; set; }

        public string? OwnerNational { get; set; }

        public string? OwnerAddress { get; set; }

        public string? OwnerTaxCode { get; set; }

        public string? OwnerType { get; set; }

        public DateTime? OwnerCreatedTime { get; set; }

        public string? OwnerCreatedBy { get; set; }

        public string? ProjectId { get; set; }

        public string? PlanId { get; set; }

        public string? OwnerStatus { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? PublishedPlace { get; set; }

        public string? HusbandWifeName { get; set; }

        public string? RepresentPerson { get; set; }

        public DateTime? TaxPublishedDate { get; set; }

        public string? OrganizationTypeId { get; set; }

        public OrganizationTypeReadDTO? OrganizationType { get; set; }
    }

    public class PlansInProjectDTO
    {
        public string PlanId { get; set; } = null!;

        public string? ProjectId { get; set; }

        public string PlanReportInfo { get; set; }

        public string PlanLocation { get; set; }

        public string? PlanCode { get; set; }

        public string? PlanDescription { get; set; }

        public string? PlanCreateBase { get; set; }

        public string? PlanApprovedBy { get; set; }

        public string? PlanReportSignal { get; set; }

        public DateTime? PlanReportDate { get; set; }

        public DateTime? PlanCreatedTime { get; set; }

        public DateTime? PlanEndedTime { get; set; }

        public string? PlanCreatedBy { get; set; }

        public string? PlanStatus { get; set; }

        public string? RejectReason { get; set; }

        //Tong Chu So Huu Ho Tro Boi Thuong
        public int? TotalOwnerSupportCompensation { get; set; }

        //Tong Kinh Phi Boi Thuong
        public decimal? TotalPriceCompensation { get; set; }

        //Tong King Phi Boi Thuong Dat
        public decimal? TotalPriceLandSupportCompensation { get; set; }

        public decimal? TotalPriceHouseSupportCompensation { get; set; }

        public decimal? TotalPriceArchitectureSupportCompensation { get; set; }

        public decimal? TotalPricePlantSupportCompensation { get; set; }

        public decimal? TotalPriceOtherSupportCompensation { get; set; }

        //Tong Khau Tru
        public decimal? TotalDeduction { get; set; }

        //Tong dien tich thu hoi dat
        public decimal TotalLandRecoveryArea { get; set; }

        public decimal TotalGpmbServiceCost { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class DocumentsInProject
    {

    }

    public class UnitPriceLandsInProject
    {

    }
}
