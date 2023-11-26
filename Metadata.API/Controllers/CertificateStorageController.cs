using Metadata.Infrastructure.Services.Interfaces;
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
        private readonly IDigitalSignatureService _digitalSignatureService;

        public CertificateStorageController(IConfiguration configuration, IDigitalSignatureService digitalSignatureService)
        {
            _storagePath = configuration["StoragePath"]!;
            _digitalSignatureService = digitalSignatureService;
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

        /// <summary>
        /// Sign Document Async
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentFile"></param>
        /// <param name="signaturePassword"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignDocumentAsync(string userId, IFormFile documentFile, string signaturePassword)
        {
            var signedDocument = await _digitalSignatureService.SignDocumentAsync(userId, documentFile, signaturePassword);

            return File(signedDocument.FileByte, signedDocument.FileType, signedDocument.FileName);
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
