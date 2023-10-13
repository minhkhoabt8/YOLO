using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Owner;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDocumentService
    {
        Task AssignDocumentsToProjectAsync(string projectId, string documentId);
        Task<IEnumerable<DocumentReadDTO>> CreateDocumentsAsync(IEnumerable<DocumentWriteDTO> documentDtos);
        Task<DocumentReadDTO> CreateDocumentAsync(DocumentWriteDTO documentDto);
        Task DeleteDocumentAsync(string documentId);
        Task<DocumentReadDTO> UpdateDocumentAsync(string documentId, DocumentWriteDTO dto);
        Task<PaginatedResponse<DocumentReadDTO>> QueryDocumentAsync(DocumentQuery query);
    }
}
