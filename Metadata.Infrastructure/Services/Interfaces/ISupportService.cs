using Metadata.Infrastructure.DTOs.Support;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ISupportService
    {
        Task<IEnumerable<SupportReadDTO>> CreateOwnerSupportsAsync(string ownerId, IEnumerable<SupportWriteDTO> dto);
        Task<SupportReadDTO> UpdateSupportAsync(string supportId, SupportWriteDTO dto);
        Task DeleteSupportAsync(string supportId);
        Task<IEnumerable<SupportReadDTO>> GetSupportsAsync(string ownerId);
        Task<PaginatedResponse<SupportReadDTO>> QuerySupportAsync(SupportQuery paginationQuery);

    }
}
