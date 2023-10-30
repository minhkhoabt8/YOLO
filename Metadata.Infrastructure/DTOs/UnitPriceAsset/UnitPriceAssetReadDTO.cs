using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.UnitPriceAsset
{
    public class UnitPriceAssetReadDTO
    {
        public string UnitPriceAssetId { get; set; } 

        public string AssetName { get; set; } 

        public decimal AssetPrice { get; set; }

        public string? AssetRegulation { get; set; }

        public string AssetType { get; set; }

        public string PriceAppliedCodeId { get; set; }

        public string AssetUnitId { get; set; }

        public string AssetGroupId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
