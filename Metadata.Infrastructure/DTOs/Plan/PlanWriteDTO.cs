using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class PlanWriteDTO
    {
        [Required]
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

        public bool? PlanStatus { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
