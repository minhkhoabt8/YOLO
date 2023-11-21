using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AttachFile
{
    public class AttachFileReadDTO
    {
        public string? AttachFileId { get; set; }

        public string? Name { get; set; }

        public string? FileType { get; set; }

        public string? ReferenceLink { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string? CreatedBy { get; set; }

        public string? PlanId { get; set; }

        public string? GcnLandInfoId { get; set; }

        public string? OwnerId { get; set; }

        public string? MeasuredLandInfoId { get; set; }

        public bool? IsAssetCompensation { get; set; }
    }
}
