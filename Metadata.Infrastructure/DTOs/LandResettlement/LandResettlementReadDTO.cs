using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.LandResettlement
{
    public class LandResettlementReadDTO
    {
        public string? LandResettlementId { get; set; }

        public string? Position { get; set; }

        public string? PlotNumber { get; set; }

        public string? PageNumber { get; set; }

        public string? PlotAddress { get; set; }

        public decimal? LandSize { get; set; }

        public decimal? TotalLandPrice { get; set; }
        public string? ResettlementReason { get; set; }

        public string? ResettlementProjectId { get; set; } 

        public string? OwnerId { get; set; } 

        public OwnerReadDTO? Owner { get; set; }

        public ResettlementProjectReadDTO? ResettlementProject { get; set; }
    }
}
