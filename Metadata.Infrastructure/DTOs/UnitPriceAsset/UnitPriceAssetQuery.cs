using Metadata.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharedLib.Infrastructure.DTOs;
using System.ComponentModel.DataAnnotations;


namespace Metadata.Infrastructure.DTOs.UnitPriceAsset
{
    public class UnitPriceAssetQuery : PaginatedQuery , ISearchTextQuery, IOrderedQuery
    {
        [Required]
        public string PriceAppliedCodeId { get; set; } = null!;
        public string? Include { get; set; }
        public string? SearchText { get; set; }
        public string? OrderBy { get; set; }
        [EnumDataType(typeof(AssetOnLandTypeEnum))]
        [JsonConverter(typeof(StringEnumConverter))]
        public AssetOnLandTypeEnum? Type { get; set; }
    }
}
