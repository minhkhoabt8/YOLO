using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.UnitPriceLand
{
    public class UnitPriceLandWriteDTO
    {
        [Required]
        public string ProjectId { get; set; } = null!;
        [Required]
        public string StreetAreaName { get; set; } = null!;
        [Required]
        public string LandTypeId { get; set; } = null!;
        [Required]
        public string LandUnit { get; set; } = null!;

        public decimal? LandPosition1 { get; set; }

        public decimal? LandPosition2 { get; set; }

        public decimal? LandPosition3 { get; set; }

        public decimal? LandPosition4 { get; set; }

        public decimal? LandPosition5 { get; set; }
    }
}
