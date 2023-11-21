using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("storage/certificate")]
    public class CertificateStorageController : ControllerBase
    {
        private readonly string _storagePath;

        public CertificateStorageController(IConfiguration configuration)
        {
            _storagePath = configuration["StoragePath"]!;
        }

        [HttpPost]
        public string UploadFile([Required] UploadDTO dto)
        {
            var filePath = Path.Combine(_storagePath, dto.Name);

            if (System.IO.File.Exists(filePath))
            {
                throw new Exception("File already exists");
            }

            System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(dto.Data));

            return Url.Action(nameof(GetFile), new { name = dto.Name })!;
        }


        [HttpGet("{name}")]
        public IActionResult GetFile(string name)
        {
            var filePath = Path.Combine(_storagePath, name);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream");
        }



        [HttpDelete("{name}")]
        public NoContentResult DeleteFile(string name)
        {
            var filePath = Path.Combine(_storagePath, name);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return NoContent();
        }

        public struct UploadDTO
        {
            [Required] public string Name { get; set; }
            [Required] public string Data { get; set; }
        }

    }
}
