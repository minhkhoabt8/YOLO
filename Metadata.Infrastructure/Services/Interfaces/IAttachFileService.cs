using Metadata.Infrastructure.DTOs.AttachFile;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAttachFileService
    {
        Task<IEnumerable<AttachFileReadDTO>> CreateOwnerAttachFilesAsync(string ownerId, IEnumerable<AttachFileWriteDTO> dto);
        Task<AttachFileReadDTO> UpdateAttachFileAsync(string fileId, AttachFileWriteDTO dto);
        Task DeleteAttachFileAsync(string fileId);
    }
}
