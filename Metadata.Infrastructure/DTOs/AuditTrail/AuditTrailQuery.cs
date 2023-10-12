using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AuditTrail
{
    public class AuditTrailQuery : PaginatedQuery, ISearchTextQuery, IOrderedQuery
    {
        public string? SearchText { get; set; }
        public string? OrderBy { get; set; }
    }
}
