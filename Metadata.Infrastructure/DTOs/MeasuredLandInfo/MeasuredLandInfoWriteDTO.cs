using Metadata.Infrastructure.DTOs.AttachFile;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.MeasuredLandInfo
{
    public class MeasuredLandInfoWriteDTO
    {
        public string? MeasuredPageNumber { get; set; }

        public string? MeasuredPlotNumber { get; set; }

        public string? MeasuredPlotAddress { get; set; }

        public string LandTypeId { get; set; } = null!;
        [Range(0, double.MaxValue)]
        public decimal? MeasuredPlotArea { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? WithdrawArea { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? CompensationPrice { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? CompensationRate { get; set; }
        public string? CompensationNote { get; set; }
        public string GcnLandInfoId { get; set; } = null!;

        public string OwnerId { get; set; } = null!;

        public string UnitPriceLandId { get; set; } = null!;

        public IEnumerable<AttachFileWriteDTO>? AttachFiles { get; set; }
    }
}
