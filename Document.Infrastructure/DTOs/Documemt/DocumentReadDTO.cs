using Document.Infrastructure.DTOs.FileVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document.Infrastructure.DTOs.Documemt
{
    public class DocumentReadDTO
    {
        public string DocumentId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string CreatedBy { get; set; }
        public IEnumerable<FileVersionReadDTO>? FileVersions { get; set; }
    }
}
