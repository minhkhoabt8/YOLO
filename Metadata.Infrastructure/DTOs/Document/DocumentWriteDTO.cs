using Metadata.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Document
{
    public class DocumentWriteDTO
    {
        [Required]
        [MaxLength(50)]
        public string DocumentTypeId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Number { get; set; }
        [Required]
        [MaxLength(10)]
        public string Notation { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }

        public DateTime? EffectiveDate { get; set; }
        [Required]
        public string? Epitome { get; set; }

        public string? SignInfo { get; set; }

        public string? Note { get; set; }

        public string? Pen { get; set; }

        public bool? IsPublic { get; set; } = false;

        [Required]
        public string? FileName { get; set; }

        [Required]
        public FileTypeEnum FileType { get; set; }

        public byte[] FileAttach { get; set; }

    }
}
