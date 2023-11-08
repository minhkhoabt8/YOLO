using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.PriceAppliedCode
{
    public class PriceAppliedCodeWriteDTO
    {
        
        public string? UnitPriceCode { get; set; } = null!;

        public string? PriceContent { get; set; } = null!;

        public DateTime? ExpriredTime { get; set; }

        public IEnumerable<UnitPriceAssetWriteDTO> UnitPriceAssets { get; set; }

    }
}
