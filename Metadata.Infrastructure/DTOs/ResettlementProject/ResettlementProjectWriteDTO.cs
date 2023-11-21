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
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal LimitToResettlement { get; set; } = 0;

        public decimal LimitToConsideration { get; set; } = 0;

        public string? Position { get; set; }

        public int LandNumber { get; set; }

        public string? ImplementYear { get; set; }

        public decimal LandPrice { get; set; } = 0;

        public string? Note { get; set; }

        public string? LastPersonEdit { get; set; }

        public string? DocumentId { get; set; }

        public IEnumerable<DocumentWriteDTO>? ResettlementDocuments { get; set; }
    }
}
