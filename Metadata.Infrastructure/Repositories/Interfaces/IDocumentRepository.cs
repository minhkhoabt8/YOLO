using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IDocumentRepository :
        IFindAsync<Document>,
        IAddAsync<Document>,
        IUpdate<Document>,
        IDelete<Document>,
        IQueryAsync<Document, DocumentQuery>
    {
        Task<IEnumerable<Document?>> GetDocumentsOfProjectAsync(string projectId);
        Task<IEnumerable<Document?>> GetDocumentsOfResettlemtProjectAsync(string resettlementProjectId);
    }
}
