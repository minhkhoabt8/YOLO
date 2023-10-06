using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Project
{
    public class ProjectExportFileDTO
    {
        public string FileName { get; set; }
        public byte[] FileByte { get; set; }
        public string FileType { get; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
