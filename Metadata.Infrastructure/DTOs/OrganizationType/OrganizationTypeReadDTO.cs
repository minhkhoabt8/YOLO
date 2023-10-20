using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.OrganizationType
{
    public class OrganizationTypeReadDTO
    {
        public string OrganizationTypeId { get; set; } = null!;

        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsDeleted { get; set; } 
    }
}
