using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SharedLib.Infrastructure.DTOs
{
    public class UploadFileDTO
    {
        [Required]
        public IFormFile File { get; set; } = null!;
        [Required]
        public string FileName { get; set; } = null!;
        public string FileType { get; set; }

        public void DetermineFileType()
        {
            
            if (File != null && !string.IsNullOrEmpty(File.ContentType))
            {
                FileType = File.ContentType;
            }
            else
            {
                FileType = "application/octet-stream";
            }
        }
    }
}
