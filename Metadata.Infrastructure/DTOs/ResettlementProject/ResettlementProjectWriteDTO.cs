using Metadata.Infrastructure.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.ResettlementProject
{
    public class ResettlementProjectWriteDTO
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public decimal LimitToResettlement { get; set; } = 0;

        public decimal LimitToConsideration { get; set; } = 0;

        public string? Position { get; set; }

        public int LandNumber { get; set; }

        public int? ImplementYear { get; set; } = 2023;

        public decimal LandPrice { get; set; } = 0;

        public string? Note { get; set; }

        public IEnumerable<DocumentWriteDTO>? ResettlementDocuments { get; set; }
    }
}
