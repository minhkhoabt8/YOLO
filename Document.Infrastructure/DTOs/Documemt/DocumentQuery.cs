using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document.Infrastructure.DTOs.Documemt
{
    public class DocumentQuery : PaginatedQuery
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
