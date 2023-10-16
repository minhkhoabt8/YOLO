using Document.Infrastructure.DTOs.FileVersion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document.Infrastructure.DTOs.Documemt
{
    public class DocumentWriteDTO
    {
        [Required]
        public string DocumentId { get; set; }
        [Required]
        public string Title { get; set; } 
        public string? Description { get; set; }
        [Required]
        public string CreatedBy { get; set; }

    }
}
