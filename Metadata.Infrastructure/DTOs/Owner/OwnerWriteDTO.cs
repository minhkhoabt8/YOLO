﻿using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Support;
using System.ComponentModel.DataAnnotations;


namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerWriteDTO
    {

        [MaxLength(20)]
        public string OwnerCode { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [MaxLength(20)]
        public string OwnerIdCode { get; set; }
        [MaxLength(10)]
        public string OwnerGender { get; set; }
        public DateTime? OwnerDateOfBirth { get; set; }
        [MaxLength(50)]
        public string OwnerEthnic { get; set; }
        [MaxLength(50)]
        public string OwnerNational { get; set; }
        [MaxLength(200)]
        public string OwnerAddress { get; set; }
        [MaxLength(10)]
        public string OwnerTaxCode { get; set; }
        [MaxLength(20)]
        public string OwnerType { get; set; }
        [MaxLength(50)]
        public string ProjectId { get; set; }
        [MaxLength(50)]
        public string PlanId { get; set; }
        [MaxLength(10)]
        public string OwnerStatus { get; set; }
        public DateTime PublishedDate { get; set; }
        [MaxLength(50)]
        public string PublishedPlace { get; set; }
        [MaxLength(50)]
        public string HusbandWifeName { get; set; }
        [MaxLength(50)]
        public string RepresentPerson { get; set; }

        public DateTime TaxPublishedDate { get; set; }
        [MaxLength(50)]
        public string OrganizationTypeId { get; set; }

        public IEnumerable<SupportWriteDTO>? Supports { get; set; }
        public IEnumerable<DeductionWriteDTO>? Deductions { get; set; }
        public IEnumerable<GCNLandInfoWriteDTO>? GcnlandInfos { get; set;}
        public IEnumerable<MeasuredLandInfoWriteDTO>? MeasuredLandInfos { get; set; }
        public IEnumerable<AssetCompensationWriteDTO>? AssetCompensations { get; set; }
        public IEnumerable<AttachFileWriteDTO>? AttachFiles { get; set; }

    }

}
