using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharedLib.Core.Attributes;
using System.ComponentModel.DataAnnotations;


namespace Metadata.Infrastructure.DTOs.Project
{
    public class ProjectWriteDTO
    {
        [Required]
        [MaxLength(50)]
        public string ProjectCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string ProjectName { get; set; }
        [MaxLength(200)]
        public string ProjectLocation { get; set; }
        [MaxLength(20)]
        public string? Province { get; set; }
        [MaxLength(20)]
        public string? District { get; set; }
        [MaxLength(20)]
        public string? Ward { get; set; }

        [InputType(typeof(decimal))]
        public decimal ProjectExpense { get; set; }

        [InputType(typeof(DateTime))]
        public DateTime ProjectApprovalDate { get; set; }

        [InputType(typeof(DateTime))]
        public DateTime? ProjectCreatedTime { get; set; }
        [Range(0, 9999)]
        public int ImplementationYear { get; set; } = 2023;
        [MaxLength(20)]
        public string? RegulatedUnitPrice { get; set; }
        [Range(0, int.MaxValue)]
        public int? ProjectBriefNumber { get; set; } = 0;
        [MaxLength(50)]
        public string? ProjectNote { get; set; }
        [MaxLength(50)]
        public string? PriceAppliedCodeId { get; set; }
        [MaxLength(50)]
        public string? ResettlementProjectId { get; set; }
        [MaxLength(20)]
        public string? CheckCode { get; set; }
        public string? ReportSignal { get; set; }
        [Range(0, int.MaxValue)]
        public int? ReportNumber { get; set; } = 0;
        public string? PriceBasis { get; set; }
        public string ?LandCompensationBasis { get; set; }

        public string? AssetCompensationBasis { get; set; }

        [EnumDataType(typeof(ProjectStatusEnum))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectStatusEnum? ProjectStatus { get; set; } = ProjectStatusEnum.INPROGRESS;
        public IEnumerable<LandPositionInfoWriteDTO>? LandPositionInfos { get; set; }
        public ResettlementProjectWriteDTO? ResettlementProject { get; set; }
        //public IEnumerable<UnitPriceLandInProjectWriteDTO> UnitPriceLands { get; set; }
        public IEnumerable<DocumentWriteDTO>? Documents { get; set; }
        
    }

    public class DocumentInProjectWriteDTO
    {
        public string? DocumentTypeId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Number { get; set; }
        [Required]
        [MaxLength(10)]
        public string Notation { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        [Required]
        public string Epitome { get; set; }

        public string? SignInfo { get; set; }

        public string? Note { get; set; }

        public string? Pen { get; set; }

        public bool? IsPublic { get; set; } = false;

        [Required]
        public IFormFile? FileAttach { get; set; }
    }

    public class LandPositionInfoInProjectWriteDTO
    {
        [Required]
        public string? LocationName { get; set; }

        public string? Description { get; set; }
        [Required]
        public string? LandInfoType { get; set; }
    }

    public class UnitPriceLandInProjectWriteDTO
    {
        [Required]
        public string StreetAreaName { get; set; } = null!;
        [Required]
        public string LandTypeId { get; set; } = null!;
        [Required]
        public string LandUnit { get; set; } = null!;

        public decimal? LandPosition1 { get; set; }

        public decimal? LandPosition2 { get; set; }

        public decimal? LandPosition3 { get; set; }

        public decimal? LandPosition4 { get; set; }

        public decimal? LandPosition5 { get; set; }
    }

}
