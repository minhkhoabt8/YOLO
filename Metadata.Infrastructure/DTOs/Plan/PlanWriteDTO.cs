using DocumentFormat.OpenXml.Wordprocessing;
using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharedLib.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class PlanWriteDTO
    {
        [Required]
        [MaxLength(50)]
        public string ProjectId { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string PlanReportInfo { get; set; }

        [MaxLength(10)]
        public string? PlanCode { get; set; } = null ?? "Generated";
        
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

        [EnumDataType(typeof(PlanStatusEnum))]
        [JsonConverter(typeof(StringEnumConverter))]
        public PlanStatusEnum? PlanStatus { get; set; } = PlanStatusEnum.DRAFT;
        //public IEnumerable<OwnerWriteDTO>? Owners { get; set; }
        public IEnumerable<AttachFileWriteDTO>? AttachFiles { get; set; }
    }
}
