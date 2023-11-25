using Metadata.Core.Enums;
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
        public string? PlanId { get; set; } = null;

        public string? GcnLandInfoId { get; set; } = null;

        public string? MeasuredLandInfoId { get; set; } = null;

        public string? OwnerId { get; set; } = null;

        public bool IsAssetCompensation { get; set; }

        public string? Name { get; set; }
        public FileTypeEnum? FileType { get; set; }
        public byte[]? AttachFile { get; set; }
        
    }
}
