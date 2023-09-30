using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.PriceAppliedCode
{
    public class PriceAppliedCodeReadDTO
    {
        public string PriceAppliedCodeId { get; set; }

        public string? UnitPriceCode { get; set; }

        public string? PriceContent { get; set; }

        public DateTime? ExpriredTime { get; set; }
    }
}
