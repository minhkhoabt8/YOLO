using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.DTOs.Account
{
    public class AccountQuery : PaginatedQuery, IIncludeQuery, ISearchTextQuery, IActiveQuery
    {
        public string? Include { get; set; }
        public bool? ShowInactive { get; set; } = false;
        public string? SearchText { get; set; }
        public string? OrderBy { get; set; } 
        public string? RoleId { get; set; }
    }
}
