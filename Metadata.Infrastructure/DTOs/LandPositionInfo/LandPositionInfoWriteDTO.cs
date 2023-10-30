using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.LandPositionInfo
{
    public class LandPositionInfoWriteDTO
    {
        [Required]
        public string LocationName { get; set; }

        public string? Description { get; set; }
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string LandInfoType { get; set; }
    }
}
