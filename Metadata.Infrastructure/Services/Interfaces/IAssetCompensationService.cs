using Metadata.Infrastructure.DTOs.AssetCompensation;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAssetCompensationService
    {
        Task<IEnumerable<AssetCompensationReadDTO>> CreateOwnerAssetCompensationsAsync(string ownerId, IEnumerable<AssetCompensationWriteDTO> dto);
        Task<AssetCompensationReadDTO> UpdateAssetCompensationAsync(string compensationId, AssetCompensationWriteDTO dto);
        Task DeleteAssetCompensationAsync(string compensationId);
    }
}
