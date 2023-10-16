using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.MeasuredLandInfo
{
    public class MeasuredLandInfoWriteDTO
    {
        [Required]
        [MaxLength(10)]
        public string MeasuredPageNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string MeasuredPlotNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string MeasuredPlotAddress { get; set; }
        [Required]
        [MaxLength(50)]
        public string LandTypeId { get; set; }
        [Required]
        [MaxLength(20)]
        public string MeasuredPlotArea { get; set; }
        [Required]
        [MaxLength(20)]
        public string WidthdrawArea { get; set; }
        [Required]
        [MaxLength(50)]
        public string GcnLandInfoId { get; set; }
        [Required]
        [MaxLength(50)]
        public string OwnerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string UnitPriceLandId { get; set; }
    }
}
