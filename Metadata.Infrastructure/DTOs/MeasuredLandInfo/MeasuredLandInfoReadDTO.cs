using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.LandType;

namespace Metadata.Infrastructure.DTOs.MeasuredLandInfo
{
    public class MeasuredLandInfoReadDTO
    {
        public string MeasuredLandInfoId { get; set; }
        public string? MeasuredPageNumber { get; set; }

        public string? MeasuredPlotNumber { get; set; }

        public string? MeasuredPlotAddress { get; set; }

        public string LandTypeId { get; set; } = null!;

        public decimal? MeasuredPlotArea { get; set; }

        public decimal? WithdrawArea { get; set; }

        public decimal? CompensationPrice { get; set; }

        public decimal? CompensationRate { get; set; }

        public decimal? UnitPriceLandCost { get; set; }

        public string? CompensationNote { get; set; }

        public string GcnLandInfoId { get; set; }

        public string OwnerId { get; set; } = null!;

        public string UnitPriceLandId { get; set; }

        public bool IsDeleted { get; set; }

        public LandTypeReadDTO LandType { get; set; } = null!;


        public IEnumerable<AttachFileReadDTO>? AttachFiles { get; set; }
    }
}
