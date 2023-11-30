using Metadata.Infrastructure.DTOs.LandType;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ILandTypeService
    {
        Task<IEnumerable<LandTypeReadDTO>> GetAllLandTypeAsync();
        Task<LandTypeReadDTO?> GetAsync(string code);
        Task<LandTypeReadDTO?> CreateLandTypeAsync(LandTypeWriteDTO landTypeWriteDTO);
        Task<LandTypeReadDTO?> UpdateAsync(string id, LandTypeWriteDTO landTypeUpdateDTO);
        Task<bool> DeleteAsync(string delete);
        Task<IEnumerable<LandTypeReadDTO>> GetAllActivedLandTypeAsync();
        Task<IEnumerable<LandTypeReadDTO>> CreateLandTypesAsync(IEnumerable<LandTypeWriteDTO> landTypeWriteDTOs);
        Task CheckNameLandGroupNotDuplicate(string name);
        Task CheckCodeLandGroupNotDuplicate(string code);
        Task<PaginatedResponse<LandTypeReadDTO>> QueryLandTypeAsync(LandTypeQuery paginationQuery);
        Task<IEnumerable<LandTypeReadDTO>> GetAllDeletedLandTypeAsync();
        Task<List<LandTypeReadDTO>> ImportLandTypeFromExcelAsync(string filePath);
    }
}
