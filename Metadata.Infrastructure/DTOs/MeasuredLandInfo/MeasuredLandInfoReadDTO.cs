using Metadata.Core.Entities;

namespace Metadata.Infrastructure.DTOs.MeasuredLandInfo
{
    public class MeasuredLandInfoReadDTO
    {
        public string MeasuredLandInfoId { get; set; }

        public string MeasuredPageNumber { get; set; }

        public string MeasuredPlotNumber { get; set; }

        public string MeasuredPlotAddress { get; set; }

        public string LandTypeId { get; set; }

        public string MeasuredPlotArea { get; set; }

        public string WidthdrawArea { get; set; }

        public string GcnLandInfoId { get; set; }

        public string OwnerId { get; set; }

        public string UnitPriceLandId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
