using SharedLib.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class PlanWriteDTO
    {
        [Required]
        public string ProjectId { get; set; } = null!;

        public string? PlaneCode { get; set; }

        public string? PlanPhrase { get; set; }

        public string? PlanDescription { get; set; }

        public string? PlanCreateBase { get; set; }

        [Required]
        public string PlanApprovedBy { get; set; } = null!;

        public string? PlanReportSignal { get; set; }
        [InputType(typeof(DateTime))]
        public DateTime? PlanReportDate { get; set; }
        [InputType(typeof(DateTime))]
        public DateTime? PlanCreatedTime { get; set; }
        [InputType(typeof(DateTime))]
        public DateTime? PlanEndedTime { get; set; }

        public bool? PlanStatus { get; set; }
    }
}
