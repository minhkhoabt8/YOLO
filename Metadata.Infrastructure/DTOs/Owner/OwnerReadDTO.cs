using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerReadDTO
    {
        public string OwnerId { get; set; }

        public string? OwnerCode { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerIdCode { get; set; }

        public string? OwnerGender { get; set; }

        public DateTime? OwnerDateOfBirth { get; set; }

        public string? OwnerEthnic { get; set; }

        public string? OwnerNational { get; set; }

        public string? OwnerAddress { get; set; }

        public string? OwnerTaxCode { get; set; }

        public string? OwnerType { get; set; }

        public DateTime? OwnerCreatedTime { get; set; }

        public string? OwnerCreatedBy { get; set; }

        public string? ProjectId { get; set; }

        public string? PlanId { get; set; }

        public string? OwnerStatus { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? PublishedPlace { get; set; }

        public string? HusbandWifeName { get; set; }

        public string? RepresentPerson { get; set; }

        public DateTime? TaxPublishedDate { get; set; }

        public string? OrganizationTypeId { get; set; }

        /*public  OrganizationType? OrganizationType { get; set; }*/
        public OrganizationTypeReadDTO? OrganizationType { get; set; }

        public PlanReadDTO? Plan { get; set; }

        public ProjectReadDTO? Project { get; set; }
    }
}
