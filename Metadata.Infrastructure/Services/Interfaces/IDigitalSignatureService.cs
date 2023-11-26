using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDigitalSignatureService
    {
        Task<ExportFileDTO> SignDocumentAsync(string userId, IFormFile documentFile, string signaturePassword);
    }
}
