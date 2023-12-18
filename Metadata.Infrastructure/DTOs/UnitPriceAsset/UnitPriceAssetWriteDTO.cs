using Metadata.Core.Enums;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.UnitPriceAsset
{
    public class UnitPriceAssetWriteDTO
    {
        [Required]
        public string AssetName { get; set; }

        public decimal AssetPrice { get; set; }
        [Required]
        public string? AssetRegulation { get; set; }
        [Required]
        [EnumDataType(typeof(AssetOnLandTypeEnum))]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssetOnLandTypeEnum AssetType { get; set; }
        public string? PriceAppliedCodeId { get; set; }
        [Required]
        public string AssetUnitId { get; set; }
        [Required]
        public string AssetGroupId { get; set; }

    }
}
