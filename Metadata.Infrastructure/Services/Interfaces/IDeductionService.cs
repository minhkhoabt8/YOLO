using Metadata.Infrastructure.DTOs.Deduction;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDeductionService
    {
        Task<IEnumerable<DeductionReadDTO>> CreateOwnerDeductionsAsync(string ownerId, IEnumerable<DeductionWriteDTO> dto);
        Task<DeductionReadDTO> UpdateDeductionAsync(string deductionId, DeductionWriteDTO dto);
        Task DeleteDeductionAsync(string deductionId);
        Task<PaginatedResponse<DeductionReadDTO>> QueryDeductionAsync(DeductionQuery paginationQuery);
        Task<IEnumerable<DeductionReadDTO>> GetDeductionsAsync(string ownerId);
    }
}
