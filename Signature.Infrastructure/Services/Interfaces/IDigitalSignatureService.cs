using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;

namespace Signature.Infrastructure.Services.Interfaces
{
    public interface IDigitalSignatureService
    {
        Task<ExportFileDTO> SignDocumentAsync(string userId, IFormFile documentFile, string signaturePassword);
        Task GenerateSignerCertificateAsync(string signerId, string secretPassword);
        Task<bool> VerifySignedDocument(IFormFile signedFile);
        Task<bool> VerifySignerSignatureExistAsync(string signerId);

        Task<ExportFileDTO> SignDocumentWithPictureAsync(string userId, IFormFile documentFile, string signaturePassword, bool replaceSignatureWithPicture = false);
    }
}
