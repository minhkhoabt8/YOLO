using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class ProjectDocumentRepository : GenericRepository<ProjectDocument, YoloMetadataContext>, IProjectDocumentRepository
    {
        public ProjectDocumentRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProjectDocument>> FindByDocumentIdAsync(string documentId)
        {
            return await Task.FromResult(_context.ProjectDocuments.Where(c => c.DocumentId == documentId));
        }
    }
}
