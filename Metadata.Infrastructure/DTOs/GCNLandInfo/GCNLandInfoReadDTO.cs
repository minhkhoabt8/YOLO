using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Owner;

namespace Metadata.Infrastructure.DTOs.GCNLandInfo
{
    public class GCNLandInfoReadDTO
    {
        public string GcnLandInfoId { get; set; }

        public string GcnPageNumber { get; set; }

        public string GcnPlotNumber { get; set; }

        public string GcnPlotAddress { get; set; }

        public string LandTypeId { get; set; }

        public decimal GcnPlotArea { get; set; }

        public string GcnOwnerCertificate { get; set; }

        public string OwnerId { get; set; }

        public LandTypeReadDTO? LandType { get; set; }

        public IEnumerable<AttachFileReadDTO> AttachFiles { get; set; }


    }
}
