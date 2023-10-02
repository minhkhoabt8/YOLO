using Metadata.Core.Entities;
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
        IDelete<Document>
    {
        Task<IEnumerable<Document?>> GetDocumentsOfProjectAsync(string projectId);
    }
}
