using Metadata.Infrastructure.DTOs.Owner;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerReadDTO> GetOwnerAsync(string ownerId);
        Task<PaginatedResponse<OwnerReadDTO>> QueryOwnerAsync(OwnerQuery query);
        Task<OwnerReadDTO> CreateOwnerAsync(OwnerWriteDTO dto);
        Task<OwnerReadDTO> UpdateOwnerAsync(string ownerId, OwnerWriteDTO dto);
        Task DeleteOwner(string ownerId);
        Task ImportOwner(IFormFile attachFile);
        Task<ExportFileDTO> ExportOwnerFileAsync(string projectId);
    }
}
