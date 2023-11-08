using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.UnitPriceLand
{
    public class UnitPriceLandReadDTO
    {
        public string UnitPriceLandId { get; set; } = null!;

        public string? ProjectId { get; set; }

        public string? StreetAreaName { get; set; }

        public string? LandTypeId { get; set; }

        public string? LandUnit { get; set; }

        public decimal? LandPosition1 { get; set; }

        public decimal? LandPosition2 { get; set; }

        public decimal? LandPosition3 { get; set; }

        public decimal? LandPosition4 { get; set; }

        public decimal? LandPosition5 { get; set; }

        public virtual LandTypeReadDTO? LandType { get; set; }

    }
}
