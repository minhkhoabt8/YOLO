using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.DTOs.Project
{
    public class ProjectQuery : PaginatedQuery, IIncludeQuery, ISearchTextQuery, IOrderedQuery
    {
        public string? Include { get; set; }
        public string? SearchText { get; set; }
        public string? OrderBy { get; set; }
    }
}
