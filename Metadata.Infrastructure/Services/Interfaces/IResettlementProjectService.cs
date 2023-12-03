using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IResettlementProjectService
    {
        Task<ResettlementProjectReadDTO> CreateResettlementProjectAsync(ResettlementProjectWriteDTO dto);
        Task<IEnumerable<ResettlementProjectReadDTO>> CreateResettlementProjectsAsync(IEnumerable<ResettlementProjectWriteDTO> dto);
        Task<IEnumerable<ResettlementProjectReadDTO>> GetAllResettlementProjectsAsync();
        Task<ResettlementProjectReadDTO> UpdateResettlementProjectAsync(string id, ResettlementProjectWriteDTO dto);
        Task DeleteResettlementProjectAsync(string id);
        Task<ResettlementProjectReadDTO> GetResettlementProjectAsync(string id);
        Task<PaginatedResponse<ResettlementProjectReadDTO>> ResettlementProjectQueryAsync(ResettlementProjectQuery query);
        Task<ResettlementProjectReadDTO> GetResettlementProjectByProjectIdAsync(string projectId);
        Task<ResettlementProjectReadDTO> CreateResettlementProjectDocumentsAsync(string resettlementId, IEnumerable<DocumentWriteDTO> documentDtos);
        Task<bool> CheckCodeResettlementProjectNotDuplicateAsync(string code);
        Task<bool> CheckNameResettlementProjectNotDuplicateAsync(string name);
    }
}
