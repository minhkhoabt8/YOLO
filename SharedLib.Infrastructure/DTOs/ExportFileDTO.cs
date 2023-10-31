using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Infrastructure.DTOs
{
    public class ExportFileDTO
    {
        public string FileName { get; set; }
        public byte[] FileByte { get; set; }
        public string FileType { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
