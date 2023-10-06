using Metadata.Infrastructure.DTOs.Document;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDocumentService
    {
        Task AssignDocumentsToProjectAsync(string projectId, string documentId);
        Task<IEnumerable<DocumentReadDTO>> CreateDocumentsAsync(IEnumerable<DocumentWriteDTO> documentDtos);
        Task<DocumentReadDTO> CreateDocumentAsync(DocumentWriteDTO documentDto);
    }
}
