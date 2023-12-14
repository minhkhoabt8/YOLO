using SharedLib.Infrastructure.DTOs;
using System.ComponentModel;

namespace Metadata.Infrastructure.DTOs.PriceAppliedCode
{
    public class PriceAppliedCodeQuery : PaginatedQuery, IIncludeQuery, ISearchTextQuery, IOrderedQuery
    {
        public string? Include { get; set; }
        public string? SearchText { get; set; }
        public string? OrderBy { get; set; }
        [DefaultValue(false)]
        public bool? IsExpired { get; set; } = false;
    }
}
