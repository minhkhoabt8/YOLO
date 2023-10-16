﻿using Metadata.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AssetCompensation
{
    public class AssetCompensationWriteDTO
    {
        public string CompensationContent { get; set; }
        [Required]
        [MaxLength(20)]
        public string CompensationRate { get; set; }
        [Required]
        public int QuantityArea { get; set; }
        [Required]
        [MaxLength(20)]
        public string CompensationUnit { get; set; }
        [Required]
        public decimal CompensationPrice { get; set; }
        [Required]
        [MaxLength(20)]
        public string CompensationType { get; set; }
        [Required]
        [MaxLength(50)]
        public string UnitPriceAssetId { get; set; }
        [Required]
        [MaxLength(50)]
        public string OwnerId { get; set; }
        [Required]
        public IEnumerable<AttachFile>? AttachFiles { get; set; }
    }
}