using Microsoft.AspNetCore.Http;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AttachFile
{
    public class AttachFileWriteDTO
    {
        public string? PlanId { get; set; }

        public string? GcnLandInfoId { get; set; }

        public string? MeasuredLandInfoId { get; set; }

        public string? OwnerId { get; set; }

        public string? AssetCompensationId { get; set; }
        [Required]
        public byte[] AttachFile { get; set; }

    }
}
