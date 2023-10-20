using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DeductionType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Deduction
{
    public class DeductionReadDTO
    {
        public string DeductionId { get; set; } 

        public string DeductionContent { get; set; }

        public decimal DeductionPrice { get; set; }

        public string OwnerId { get; set; }

        public string DeductionTypeId { get; set; }

        public DeductionTypeReadDTO? DeductionType { get; set; }
    }
}
