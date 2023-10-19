using Metadata.Infrastructure.DTOs.DocumentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<IEnumerable<DocumentTypeReadDTO>> GetAllDocumentTypesAsync();
        Task<DocumentTypeReadDTO> GetDocumentTypeAsync(string id);
        Task<DocumentTypeReadDTO> CreateDocumentTypeAsync(DocumentTypeWriteDTO documentType);
        Task<DocumentTypeReadDTO> UpdateDocumentTypeAsync(string id, DocumentTypeWriteDTO documentType);
        Task<IEnumerable<DocumentTypeReadDTO>> GetAllDeletedDocumentTypesAsync();
        Task<bool> DeleteDocumentTypeAsync(string id);
    }
}
