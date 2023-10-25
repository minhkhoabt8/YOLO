using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.LandPositionInfo
{
    public class LandPositionWriteDTO
    {
        public string? LocationName { get; set; }

        public string? Description { get; set; }

        public string? ProjectId { get; set; }

        public string? LandInfoType { get; set; }
    }
}
