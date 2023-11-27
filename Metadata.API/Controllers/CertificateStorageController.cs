using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.ResponseWrapper;
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

        /// <summary>
        /// Sign Document Async
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentFile"></param>
        /// <param name="signaturePassword"></param>
        /// <returns></returns>
        [HttpPost("sign")]
        public async Task<IActionResult> SignDocumentAsync(string userId, IFormFile documentFile, string signaturePassword)
        {
            var signedDocument = await _digitalSignatureService.SignDocumentAsync(userId, documentFile, signaturePassword);

            return File(signedDocument.FileByte, signedDocument.FileType, signedDocument.FileName);
        }

        /// <summary>
        /// Create Signer Signature Async
        /// </summary>
        /// <param name="signerId"></param>
        /// <param name="secretPassword"></param>
        /// <returns></returns>
        [HttpPost("generate/certificate")]
        public async Task<IActionResult> CreateSignatureAsync([Required]string signerId, [Required]string secretPassword)
        {
            await _digitalSignatureService.GenerateSignerCertificateAsync(signerId, secretPassword);

            return ResponseFactory.NoContent();
        }

    }
}
