using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.AttachFile;
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
        Task<OwnerReadDTO> AssignProjectOwnerAsync(string projectId, string ownerId);
        Task<IEnumerable<OwnerReadDTO>> RemoveOwnerFromPlanAsync(string planId, IEnumerable<string> ownerIds);
        Task<OwnerReadDTO> RemoveOwnerFromProjectAsync(string ownerId, string projectId);
        Task<IEnumerable<OwnerReadDTO>> GetOwnersOfProjectAsync(string projectId);
        Task<OwnerReadDTO> CreateOwnerWithFullInfomationAsync(OwnerWriteDTO dto);

        Task<IEnumerable<OwnerReadDTO>> GetAllOwner();
        Task<IEnumerable<OwnerReadDTO>> ImportOwnerFromExcelFileAsync(IFormFile file);
        Task<IEnumerable<OwnerReadDTO>> AssignPlanToOwnerAsync(string planId, IEnumerable<string> ownerIds);

        Task<PaginatedResponse<OwnerReadDTO>> GetOwnerInPlanByPlanIdAndOwnerInProjectThatNotInAnyPlanByProjectIdAsync(PaginatedQuery query, string planId, string projectId);

        Task<OwnerReadDTO> UpdateOwnerStatusAsync(string ownerId, OwnerStatusEnum ownerStatus, string? rejectReason, AttachFileWriteDTO? file);
    }
}
