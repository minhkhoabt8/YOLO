using Metadata.Infrastructure.DTOs.Support;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ISupportService
    {
        Task<IEnumerable<SupportReadDTO>> CreateOwnerSupportsAsync(string ownerId, IEnumerable<SupportWriteDTO> dto);
        Task<SupportReadDTO> UpdateSupportAsync(string supportId, SupportWriteDTO dto);
        Task DeleteSupportAsync(string supportId);
    }
}
