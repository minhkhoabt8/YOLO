using Metadata.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Document
{
    public class DocumentWriteDTO
    {
        public string? Id { get; set; }

        [MaxLength(50)]
        public string? DocumentTypeId { get; set; }
        [Range(0, 9999)]
        [DefaultValue(0)]
        public int? Number { get; set; } = 0;
        [MaxLength(10)]
        public string? Notation { get; set; }
        public DateTime? PublishedDate { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string? Epitome { get; set; }

        public string? SignInfo { get; set; }

        public string? Note { get; set; }

        public string? Pen { get; set; }
        [DefaultValue(false)]
        public bool? IsPublic { get; set; } = false;
        [DefaultValue(false)]
        public bool IsUnitPriceLand { get; set; } = false;

        public string? FileName { get; set; }

        public FileTypeEnum? FileType { get; set; }

        public byte[]? FileAttach { get; set; }

    }
}
