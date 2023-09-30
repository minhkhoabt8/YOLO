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
        public string DocumentTypeId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Notation { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        [Required]
        public string Epitome { get; set; }

        public string SignInfo { get; set; }

        public string Note { get; set; }

        public string Pen { get; set; }

        public string ReferenceLink { get; set; }

        public bool? IsPublic { get; set; } = false;

        public IFormFile FileAttach { get; set; }
    }
}
