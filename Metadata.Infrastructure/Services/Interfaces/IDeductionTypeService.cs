using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.LandGroup;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDeductionTypeService
    {
        Task<IEnumerable<DeductionTypeReadDTO>> GetAllDeductionTypesAsync();
        Task<DeductionTypeReadDTO> GetDeductionTypeAsync(string id);
        Task<DeductionTypeReadDTO> AddDeductionType(DeductionTypeWriteDTO deductionType);
        Task<DeductionTypeReadDTO> UpdateDeductionTypeAsync(string id , DeductionTypeWriteDTO deductionType);
        Task<IEnumerable<DeductionTypeReadDTO>> GetActivedDeductionTypes();
        Task<bool> DeleteDeductionTypeAsync(string id);

        Task CheckcodeDeductionTypeNotDuplicate(string code);
        Task ChecknameDeductionTypeNotDuplicate(string name);
        Task<IEnumerable<DeductionTypeReadDTO>> CreateListDeductionTypes(IEnumerable<DeductionTypeWriteDTO> WriteDTOs);
        Task<PaginatedResponse<DeductionTypeReadDTO>> QueryDeductionTypesAsync(DeductionTypeQuery paginationQuery);

        Task ImportDeductionTypeFromExcelAsync(string filePath);
    }
}
