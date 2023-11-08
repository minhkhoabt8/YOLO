using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.GCNLandInfo
{
    public class GCNLandInfoWriteDTO
    {
        [Required]
        [MaxLength(10)]
        public string GcnPageNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string GcnPlotNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string GcnPlotAddress { get; set; }
        [Required]
        [MaxLength(50)]
        public string LandTypeId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal GcnPlotArea { get; set; }
        [Required]
        public string GcnOwnerCertificate { get; set; }

        public string? OwnerId { get; set; }

        public IEnumerable<MeasuredLandInfoWriteDTO>? MeasuredLandInfos { get; set; }

        public IEnumerable<AttachFileWriteDTO>? AttachFiles { get; set; }
    }
}
