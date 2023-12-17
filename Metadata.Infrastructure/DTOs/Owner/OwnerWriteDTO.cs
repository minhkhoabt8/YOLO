using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.LandResettlement;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerWriteDTO
    {
        [MaxLength(20)]
        public string? OwnerCode { get; set; } = "";
        [Required]
        public string OwnerName { get; set; }
        [MaxLength(20)]
        public string? OwnerIdCode { get; set; }
        [MaxLength(10)]
        public string? OwnerGender { get; set; } = "";
        public DateTime? OwnerDateOfBirth { get; set; }
        [MaxLength(50)]
        public string? OwnerEthnic { get; set; } = "";
        [MaxLength(50)]
        public string? OwnerNational { get; set; } = "";
        [MaxLength(200)]
        public string? OwnerAddress { get; set; } = "";
        [MaxLength(13)]
        public string? OwnerTaxCode { get; set; } = "";
        [MaxLength(20)]
        public string? OwnerType { get; set; }
        [MaxLength(50)]
        public string? ProjectId { get; set; }
        [MaxLength(50)]
        public string? PlanId { get; set; }
        [EnumDataType(typeof(OwnerStatusEnum))]
        [JsonConverter(typeof(StringEnumConverter))]
        public OwnerStatusEnum OwnerStatus { get; set; }
        public DateTime? PublishedDate { get; set; }
        [MaxLength(500)]
        public string? PublishedPlace { get; set; } = "";
        [MaxLength(50)]
        public string? HusbandWifeName { get; set; } = "";
        [MaxLength(50)]
        public string? RepresentPerson { get; set; } = "";

        public DateTime? TaxPublishedDate { get; set; }
        [MaxLength(50)]
        public string? OrganizationTypeId { get; set; }

        public IEnumerable<SupportWriteDTO>? OwnerSupports { get; set; }
        public IEnumerable<DeductionWriteDTO>? OwnerDeductions { get; set; }
        public IEnumerable<GCNLandInfoWriteDTO>? OwnerGcnlandInfos { get; set; }
        //public IEnumerable<MeasuredLandInfoWriteDTO>? MeasuredLandInfos { get; set; }
        public IEnumerable<AssetCompensationWriteDTO>? OwnerAssetCompensations { get; set; }
        public IEnumerable<LandResettlementWriteDTO>? OwnersLandResettlements { get; set; }
        public IEnumerable<AttachFileWriteDTO>? OwnerFiles { get; set; }

    }

}
