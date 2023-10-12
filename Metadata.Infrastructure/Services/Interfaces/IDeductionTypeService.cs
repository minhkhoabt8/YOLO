using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.LandGroup;
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
        Task<DeductionTypeReadDTO> AddDeductionType(LandGroupWriteDTO deductionType);
        Task<DeductionTypeReadDTO> UpdateDeductionTypeAsync(string id ,LandGroupWriteDTO deductionType);
        Task<bool> DeleteDeductionTypeAsync(string id);
    }
}
