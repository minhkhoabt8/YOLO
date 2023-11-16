using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
