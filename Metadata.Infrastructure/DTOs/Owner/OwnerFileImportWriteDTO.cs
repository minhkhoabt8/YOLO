using Metadata.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerFileImportWriteDTO
    {
        [MaxLength(20)]
        public string OwnerCode { get; set; }
        [Required]
        [MaxLength(50)]
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
        [MaxLength(13)]
        public string OwnerTaxCode { get; set; }
        [MaxLength(20)]
        public string OwnerType { get; set; }
        public DateTime? PublishedDate { get; set; }
        [MaxLength(50)]
        public string PublishedPlace { get; set; }
        [MaxLength(50)]
        public string HusbandWifeName { get; set; }
        [MaxLength(50)]
        public string RepresentPerson { get; set; }
        public DateTime? TaxPublishedDate { get; set; }
        [MaxLength(50)]
        public string OrganizationTypeId { get; set; }
        [MaxLength(50)]
        public string ProjectId { get; set; }
    }
}
