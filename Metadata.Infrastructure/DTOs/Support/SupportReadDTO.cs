using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.SupportType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Support
{
    public class SupportReadDTO
    {
        public string SupportId { get; set; }

        public string SupportContent { get; set; }

        public string AssetUnitId { get; set; }

        public int SupportNumber { get; set; }

        public decimal SupportUnitPrice { get; set; }

        public decimal SupportPrice { get; set; }

        public string OwnerId { get; set; }

        public string SupportTypeId { get; set; }

        public SupportTypeReadDTO? SupportType { get; set; }

        public AssetUnitReadDTO? AssetUnit { get; set; }
    }
}
