using Metadata.Infrastructure.DTOs.Deduction;


namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDeductionService
    {
        Task<IEnumerable<DeductionReadDTO>> CreateOwnerDeductionsAsync(string ownerId, IEnumerable<DeductionWriteDTO> dto);
        Task<DeductionReadDTO> UpdateDeductionAsync(string deductionId, DeductionWriteDTO dto);
        Task DeleteDeductionAsync(string deductionId);
    }
}
