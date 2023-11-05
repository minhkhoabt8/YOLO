using Metadata.Infrastructure.DTOs.LandGroup;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ILandGroupService
    {
        Task<IEnumerable<LandGroupReadDTO>> GetAllLandGroupAsync();
        Task<LandGroupReadDTO?> GetAsync(string code);
        Task<LandGroupReadDTO?> CreateLandgroupAsync(LandGroupWriteDTO landGroupWriteDTO);
        Task<LandGroupReadDTO?> UpdateAsync(string id, LandGroupWriteDTO landGroupUpdateDTO);
        Task<IEnumerable<LandGroupReadDTO>> GetAllActivedLandGroupAsync();
        Task<bool> DeleteAsync(string  id);
        Task<IEnumerable<LandGroupReadDTO>> CreateListLandGroupAsync(IEnumerable<LandGroupWriteDTO> landGroupWriteDTOs);
        Task CheckNameLandGroupNotDuplicate(string name);
        Task CheckCodeLandGroupNotDuplicate(string code);
        Task<PaginatedResponse<LandGroupReadDTO>> QueryLandGroupAsync(LandGroupQuery paginationQuery);
    }
}
