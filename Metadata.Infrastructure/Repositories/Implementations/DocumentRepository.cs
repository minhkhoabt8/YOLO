using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class DocumentRepository : GenericRepository<Document, YoloMetadataContext>, IDocumentRepository
    {
        public DocumentRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Document?>> GetDocumentsOfProjectAsync(string projectId)
        {
            return await _context.ProjectDocuments
                .Where(pd => pd.ProjectId == projectId)
                .Include(pd => pd.Document)
                .Select(pd => pd.Document)
                .ToListAsync();
        }
    }
}
