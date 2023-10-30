using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SharedLib.Infrastructure.DTOs
{
    public class UploadFileDTO
    {
        [Required]
        public byte[] File { get; set; } = null!;
        [Required]
        public string FileName { get; set; } = null!;
        public string FileType { get; set; }

        
    }
}
