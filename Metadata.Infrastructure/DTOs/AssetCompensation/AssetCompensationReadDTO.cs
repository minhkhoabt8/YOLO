﻿using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AssetCompensation
{
    public class AssetCompensationReadDTO
    {
        public string AssetCompensationId { get; set; }
        public string CompensationContent { get; set; }

        public string CompensationRate { get; set; }

        public int QuantityArea { get; set; }

        public string CompensationUnit { get; set; }

        public decimal CompensationPrice { get; set; }

        public string CompensationType { get; set; }

        public string UnitPriceAssetId { get; set; }

        public string OwnerId { get; set; }

        public virtual IEnumerable<AttachFile> AttachFiles { get; } = new List<AttachFile>();

    }
}