﻿using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Owner
{
    public class OwnerQuery: PaginatedQuery, IIncludeQuery, ISearchTextQuery, IOrderedQuery
    {
        public string? Include { get; set; }
        public string? SearchText { get; set; }
        public string? OrderBy { get; set; }
    }
}