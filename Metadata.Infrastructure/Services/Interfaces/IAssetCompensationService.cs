using Metadata.Infrastructure.DTOs.AssetCompensation;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAssetCompensationService
    {
        Task<IEnumerable<AssetCompensationReadDTO>> CreateOwnerAssetCompensationsAsync(string ownerId, IEnumerable<AssetCompensationWriteDTO> dto);
        Task<AssetCompensationReadDTO> UpdateAssetCompensationAsync(string compensationId, AssetCompensationWriteDTO dto);
        Task DeleteAssetCompensationAsync(string compensationId);
        Task<IEnumerable<AssetCompensationReadDTO>> GetAssetCompensationsAsync(string ownerId);
        Task<PaginatedResponse<AssetCompensationReadDTO>> QueryAssetCompensationAsync(AssetCompensationQuery paginationQuery);
    }
}
