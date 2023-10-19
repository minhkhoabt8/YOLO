
using Metadata.Infrastructure.DTOs.SupportType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ISupportTypeService
    {
        Task<IEnumerable<SupportTypeReadDTO>> GetAllLandTypeAsync();
        Task<SupportTypeReadDTO?> GetAsync(string code);
        Task<SupportTypeReadDTO?> CreateLandTypeAsync(SupportTypeWriteDTO supportTypeWriteDTO);
        Task<SupportTypeReadDTO?> UpdateAsync(string id, SupportTypeWriteDTO supportTypeUpdateDTO);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<SupportTypeReadDTO>> GetAllDeletedLandTypeAsync();
    }
}
