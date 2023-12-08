using System.ComponentModel.DataAnnotations;


namespace Metadata.Infrastructure.DTOs.UnitPriceLand
{
    public class UnitPriceLandFileImportWriteDTO
    {
        [Required]
        public string ProjectId { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string StreetAreaName { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string LandTypeId { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string LandUnit { get; set; } = null!;

        public decimal? LandPosition1 { get; set; } = 0;

        public decimal? LandPosition2 { get; set; } = 0;

        public decimal? LandPosition3 { get; set; } = 0;

        public decimal? LandPosition4 { get; set; } = 0;

        public decimal? LandPosition5 { get; set; } = 0;
    }
}
