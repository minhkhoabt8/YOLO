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
        /// <param name="replaceSignatureWithPicture"></param>
        /// <returns></returns>
        [HttpPost("signPicture")]
        public async Task<IActionResult> SignDocumentWithPictureAsync(string userId, IFormFile documentFile, string signaturePassword, bool replaceSignatureWithPicture = false)
        {
            var signedDocument = await _digitalSignatureService.SignDocumentWithPictureAsync(userId, documentFile, signaturePassword, replaceSignatureWithPicture);

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


        /// <summary>
        /// Verify Signer Signature Exist
        /// </summary>
        /// <param name="signerId"></param>
        /// <returns></returns>
        [HttpGet("verify")]
        public async Task<IActionResult> VerifySignerSignatureExistAsync([Required] string signerId)
        {
            var result = await _digitalSignatureService.VerifySignerSignatureExistAsync(signerId);

            return ResponseFactory.Accepted(result.ToString());
        }

    }
}
