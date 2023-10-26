using Metadata.Infrastructure.DTOs.AttachFile;
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
        [Range(0, double.MaxValue)]
        public decimal WidthdrawArea { get; set; } = 0;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CompensationPrice { get; set; } = 0;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CompensationRate { get; set; } = 0;
        [Required]
        [MaxLength(50)]
        public string CompensationNote { get; set; }
        [Required]
        [MaxLength(50)]
        public string GcnLandInfoId { get; set; }
        [Required]
        [MaxLength(50)]
        public string OwnerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string UnitPriceLandId { get; set; }

        public IEnumerable<AttachFileWriteDTO>? AttachFiles { get; set; }
    }
}
