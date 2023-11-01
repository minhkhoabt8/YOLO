using Metadata.Infrastructure.DTOs.LandType;
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
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<LandTypeReadDTO>> GetAllDeletedLandTypeAsync();
    }
}
