using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document.Infrastructure.DTOs.FileVersion
{
    public class FileVersionReadDTO
    {
        public string VersionId { get; set; } 

        public string DocumentId { get; set; } 
        public double VersionNumber { get; set; }

        public string? ChangeDescription { get; set; }

        public DateTime CreatedTime { get; set; }

        public string ReferenceLink { get; set; }

        public bool IsReady { get; set; }

        public bool IsDeleted { get; set; }

    }
}
