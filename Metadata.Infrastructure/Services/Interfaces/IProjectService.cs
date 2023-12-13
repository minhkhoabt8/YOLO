using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Project;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectReadDTO> CreateProjectAsync(ProjectWriteDTO projectDto);
        Task<IEnumerable<ProjectReadDTO>> GetAllProjectsAsync();
        Task<ProjectReadDTO> UpdateProjectAsync(string projectId, ProjectWriteDTO project);
        Task DeleteProjectAsync(string projectId);
        Task<ProjectReadDTO> GetProjectAsync(string projectId);
        Task CreateProjectsFromFileAsync(IFormFile formFile);
        Task<PaginatedResponse<ProjectReadDTO>> ProjectQueryAsync(ProjectQuery query);
        Task<ExportFileDTO> ExportProjectFileAsync();
        Task<ProjectReadDTO> CreateProjectDocumentsAsync(string projectId, IEnumerable<DocumentWriteDTO> documents);
        Task<IEnumerable<ProjectReadDTO>> GetProjectOfOwnerAsync(string ownerId);
        Task<bool> CheckDuplicateProjectCodeAsync(string projectCode);
        Task<bool> CheckDuplicateProjectNameAsync(string projectName);
        Task<bool> CheckProjectAvailableForEditOrDelete(string projectId);
    }
}
