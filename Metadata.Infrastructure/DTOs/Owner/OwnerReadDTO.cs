using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.LandResettlement;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.DTOs.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerReadDTO
    {
        public string OwnerId { get; set; }

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

        public string? RejectReason { get; set; }

        //public  OrganizationType? OrganizationType { get; set; }
        public OrganizationTypeReadDTO? OrganizationType { get; set; }

        public PlanInOwnerReadDTO? PlanCode { get; set; }

        //public ProjectReadDTO? Project { get; set; }
        public IEnumerable<SupportReadDTO>? Supports { get; set; }
        public IEnumerable<DeductionReadDTO>? Deductions { get; set; }
        public IEnumerable<LandResettlementInProjectReadDTO>? LandResettlements { get; set; }
        public IEnumerable<GCNLandInfoReadDTO>? GcnlandInfos { get; set; }
        public IEnumerable<MeasuredLandInfoReadDTO>? MeasuredLandInfos { get; set; }
        public IEnumerable<AssetCompensationReadDTO>? AssetCompensations { get; set; }
        public IEnumerable<AttachFileReadDTO>? AttachFiles { get; set; }

    }

    public class LandResettlementInProjectReadDTO
    {
        public string? LandResettlementId { get; set; }

        public string? Position { get; set; }

        public string? PlotNumber { get; set; }

        public string? PageNumber { get; set; }

        public string? PlotAddress { get; set; }

        public decimal? LandSize { get; set; }

        public decimal? TotalLandPrice { get; set; }

        public string? ResettlementProjectId { get; set; }

        public string? OwnerId { get; set; }
        public ResettlementProjectReadDTO? ResettlementProject { get; set; }
    }

    public class PlanInOwnerReadDTO
    {
        public string? PlanCode { get; set; }
    }
}
