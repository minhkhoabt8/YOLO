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

        public int LimitToResettlement { get; set; }

        public int LimitToConsideration { get; set; }

        public string? Position { get; set; }

        public int LandNumber { get; set; }

        public string? ImplementYear { get; set; }

        public decimal LandPrice { get; set; }

        public string? Note { get; set; }

        public string? LastPersonEdit { get; set; }

        public string DocumentId { get; set; }

        public string ProjectId { get; set; }
    }
}
