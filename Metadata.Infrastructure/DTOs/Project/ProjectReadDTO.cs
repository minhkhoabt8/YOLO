using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
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

        public ICollection<OwnerReadDTO> Owners {  get; set; }

        public ICollection<PlanReadDTO> Plans { get; set; }

        public PriceAppliedCodeReadDTO PriceAppliedCode { get; set; }
        //Add Documents manual
        public ICollection<DocumentReadDTO> Documents { get; set; }

        public ICollection<UnitPriceLandReadDTO> UnitPriceLands { get; set; }

    }
}
