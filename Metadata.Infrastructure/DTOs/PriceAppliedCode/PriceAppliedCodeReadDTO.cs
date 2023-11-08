using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;


namespace Metadata.Infrastructure.DTOs.PriceAppliedCode
{
    public class PriceAppliedCodeReadDTO
    {
        public string PriceAppliedCodeId { get; set; }

        public string? UnitPriceCode { get; set; }

        public string? PriceContent { get; set; }

        public DateTime? ExpriredTime { get; set; }

        public IEnumerable<UnitPriceAssetReadDTO> UnitPriceAssets { get; set; }
    }
}
