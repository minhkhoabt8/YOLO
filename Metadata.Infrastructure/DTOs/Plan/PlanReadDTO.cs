using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class PlanReadDTO
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

        public  ICollection<Core.Entities.AttachFile> AttachFiles { get; set; }

        public  ICollection<OwnerReadDTO> Owners { get; set; }
    }
}
