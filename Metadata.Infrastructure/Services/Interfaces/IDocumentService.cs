using Metadata.Infrastructure.DTOs.Document;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentReadDTO>> AssignDocumentsToProjectAsync(string projectId, IEnumerable<DocumentWriteDTO> documentDtos);
    }
}
