using Metadata.Infrastructure.DTOs.Project;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.ResettlementProject
{
    public class ResettlementProjectReadDTO
    {
        public string? ResettlementProjectId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public decimal? LimitToResettlement { get; set; }

        public decimal? LimitToConsideration { get; set; }

        public string? Position { get; set; }

        public int? LandNumber { get; set; }

        public int? ImplementYear { get; set; }

        public decimal LandPrice { get; set; }

        public string? Note { get; set; }

        public DateTime? LastDateEdit { get; set; }

        public string? LastPersonEdit { get; set; }

        public bool? IsDeleted { get; set; }

        public IEnumerable<ProjectInResettlementReadDTO>? Projects { get; set; }
    }

    public class ProjectInResettlementReadDTO
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

        public string? ReportSignal { get; set; }

        public int? ReportNumber { get; set; }

        public string? PriceBasis { get; set; }

        public string? LandCompensationBasis { get; set; }

        public string? AssetCompensationBasis { get; set; }

        public DateTime? ProjectCreatedTime { get; set; }

        public string? ProjectCreatedBy { get; set; }

        public string? ProjectStatus { get; set; }
    }
}
