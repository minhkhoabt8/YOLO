using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;


namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IProjectDocumentRepository : 
        IAddAsync<ProjectDocument>,
        IFindAsync<ProjectDocument>,
        IDelete<ProjectDocument>
    {
        Task<IEnumerable<ProjectDocument>> FindByDocumentIdAsync(string documentId);
    }
}
