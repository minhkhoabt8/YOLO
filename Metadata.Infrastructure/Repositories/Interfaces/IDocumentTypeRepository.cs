using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DocumentType;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
     public interface IDocumentTypeRepository : IGetAllAsync<DocumentType>,
        IFindAsync<DocumentType>,
        IAddAsync<DocumentType>,
        IDelete<DocumentType>
    {
        Task<DocumentType?> FindByCodeAsync(string code);
        Task<DocumentType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<IEnumerable<DocumentType>?> GetAllActivedDocumentTypes();
        Task<DocumentType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<DocumentType>> QueryAsync(DocumentTypeQuery query, bool trackChanges = false);
    }
}
