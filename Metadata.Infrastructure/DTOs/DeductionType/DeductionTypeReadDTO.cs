using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.DeductionType
{
    public class DeductionTypeReadDTO
    {
        public string DeductionTypeId { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool? IsDeleted { get; set; } 
    }
}
