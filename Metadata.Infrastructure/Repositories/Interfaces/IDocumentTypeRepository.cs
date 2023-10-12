using Metadata.Core.Entities;
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
    }
}
