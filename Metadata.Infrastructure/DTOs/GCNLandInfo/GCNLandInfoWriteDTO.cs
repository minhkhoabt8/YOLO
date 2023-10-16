using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(20)]
        public string GcnPlotArea { get; set; }
        [Required]
        public string GcnOwnerCertificate { get; set; }
        [Required]
        [MaxLength(50)]
        public string OwnerId { get; set; }
    }
}
