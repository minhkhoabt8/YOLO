using Metadata.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.UnitPriceAsset
{
    public class UnitPriceAssetFileImportWriteDTO
    {
        [Required]
        [MaxLength(20)]
        public string AssetName { get; set; }
        public decimal AssetPrice { get; set; } = 0;
        [Required]
        [MaxLength(20)]
        public string? AssetRegulation { get; set; }
        [Required]
        public string AssetType { get; set; }
        [Required]
        public string PriceAppliedCodeId { get; set; }
        [Required]
        public string AssetUnitId { get; set; }
        [Required]
        public string AssetGroupId { get; set; }
    }
}
