using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Infrastructure.Services.Interfaces
{
    public interface IUploadFileService
    {
        Task<string> UploadFileAsync(UploadFileDTO file);
    }
}
