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
    }
}
