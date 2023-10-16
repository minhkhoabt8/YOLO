using SharedLib.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class PlanWriteDTO
    {
        [Required]
        [MaxLength(50)]
        public string ProjectId { get; set; } = null!;
        [MaxLength(10)]
        public string? PlaneCode { get; set; } = null ?? "Generated";
        [MaxLength(10)]
        public string? PlanPhrase { get; set; }

        public string? PlanDescription { get; set; }
        [MaxLength(50)]
        public string? PlanCreateBase { get; set; }

        [Required]
        [MaxLength(50)]
        public string PlanApprovedBy { get; set; } = null!;

        public string? PlanReportSignal { get; set; }

        [InputType(typeof(DateTime))]
        public DateTime? PlanReportDate { get; set; }

        [InputType(typeof(DateTime))]
        public DateTime? PlanCreatedTime { get; set; }

        [InputType(typeof(DateTime))]
        public DateTime? PlanEndedTime { get; set; }

        public bool? PlanStatus { get; set; } = true;
    }
}
