using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Owner;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDocumentService
    {
        Task AssignDocumentsToProjectAsync(string projectId, string documentId);
        Task AssignDocumentsToResettlementProjectAsync(string resettlementProjectId, string documentId);
        Task<IEnumerable<DocumentReadDTO>> CreateDocumentsAsync(IEnumerable<DocumentWriteDTO> documentDtos);
        Task<DocumentReadDTO> CreateDocumentAsync(DocumentWriteDTO documentDto);
        Task DeleteDocumentAsync(string documentId);
        Task<DocumentReadDTO> UpdateDocumentAsync(string documentId, DocumentWriteDTO dto);
        Task<PaginatedResponse<DocumentReadDTO>> QueryDocumentAsync(DocumentQuery query);
        Task<IEnumerable<DocumentReadDTO>> GetDocumentsOfProjectAsync(string projectId);
        Task<ExportFileDTO> GetFileImportExcelTemplateAsync(string name);
        Task<DocumentReadDTO> CheckDuplicateDocumentAsync(int number, string notation, string epitome);
    }
}
