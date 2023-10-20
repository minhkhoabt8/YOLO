using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Project
{
    public class ProjectReadDTO
    {
        public string ProjectId { get; set; } 

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string ProjectLocation { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public decimal ProjectExpense { get; set; }

        public DateTime ProjectApprovalDate { get; set; }

        public string ImplementationYear { get; set; }

        public string RegulatedUnitPrice { get; set; }

        public string ProjectBriefNumber { get; set; }

        public string ProjectNote { get; set; }

        public string PriceAppliedCodeId { get; set; }

        public string CheckCode { get; set; }

        public string ReportSignal { get; set; }

        public string ReportNumber { get; set; }

        public string PriceBasis { get; set; }

        public string LandCompensationBasis { get; set; }

        public string AssetCompensationBasis { get; set; }

        public DateTime ProjectCreatedTime { get; set; }

        public string ProjectCreatedBy { get; set; }

        public bool ProjectStatus { get; set; }

        public ICollection<LandPositionInfoReadDTO> LandPositionInfos { get; set; }

        public ICollection<OwnersInProjectDTO> Owners {  get; set; }

        public ICollection<PlansInProjectDTO> Plans { get; set; }

        public PriceAppliedCodeReadDTO PriceAppliedCode { get; set; }
        //Attach Documents manual
        public IEnumerable<DocumentReadDTO>? Documents { get; set; }

        public ICollection<UnitPriceLandReadDTO>? UnitPriceLands { get; set; }

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

        public string? PlaneCode { get; set; }

        public string? PlanPhrase { get; set; }

        public string? PlanDescription { get; set; }

        public string? PlanCreateBase { get; set; }

        public string? PlanApprovedBy { get; set; }

        public string? PlanReportSignal { get; set; }

        public DateTime? PlanReportDate { get; set; }

        public DateTime? PlanCreatedTime { get; set; }

        public DateTime? PlanEndedTime { get; set; }

        public string? PlanCreatedBy { get; set; }

        public bool? PlanStatus { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class DocumentsInProject
    {

    }

    public class UnitPriceLandsInProject
    {

    }
}
