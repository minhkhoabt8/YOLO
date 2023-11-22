using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
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
                        .ThenInclude(d => d.DocumentType)
                    .Select(pd => pd.Document)
                    .ToListAsync();
        }



        public async Task<IEnumerable<Document>> QueryAsync(DocumentQuery query, bool trackChanges = false)
        {
            IQueryable<Document> documents = _context.Documents.Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                documents = documents.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                documents = documents.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                documents = documents.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                documents = documents.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Document> enumeratedDocument = documents.AsEnumerable();
            return await Task.FromResult(enumeratedDocument);
        }
    }
}
