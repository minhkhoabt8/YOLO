using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerWriteDTO
    {

        public string OwnerCode { get; set; }
        [Required]
        public string OwnerName { get; set; }

        public string OwnerIdCode { get; set; }

        public string OwnerGender { get; set; }

        public DateTime? OwnerDateOfBirth { get; set; }

        public string OwnerEthnic { get; set; }

        public string OwnerNational { get; set; }

        public string OwnerAddress { get; set; }

        public string OwnerTaxCode { get; set; }

        public string OwnerType { get; set; }

        public string ProjectId { get; set; }

        public string PlanId { get; set; }

        public string OwnerStatus { get; set; }

        public DateTime PublishedDate { get; set; }

        public string PublishedPlace { get; set; }

        public string HusbandWifeName { get; set; }

        public string RepresentPerson { get; set; }

        public DateTime TaxPublishedDate { get; set; }

        public string OrganizationTypeId { get; set; }
    }
}
