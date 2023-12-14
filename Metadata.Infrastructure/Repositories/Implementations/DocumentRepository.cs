using Metadata.Core.Data;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
using Document = Metadata.Core.Entities.Document;

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

        public async Task<IEnumerable<Document?>> GetDocumentsOfResettlemtProjectAsync(string resettlementProjectId)
        {
            return await _context.ResettlementDocuments
                    .Where(pd => pd.ResettlementProjectId == resettlementProjectId)
                    .Include(pd => pd.Document)
                        .ThenInclude(d => d.DocumentType)
                    .Select(pd => pd.Document)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Document?>> GetDocumentsOfPriceAppliedCodeAsync(string priceAppliedCodeId)
        {
            return await _context.PriceAppliedCodeDocuments
                    .Where(pd => pd.PriceAppliedCodeId == priceAppliedCodeId)
                    .Include(pd => pd.Document)
                        .ThenInclude(d => d.DocumentType)
                    .Select(pd => pd.Document)
                    .ToListAsync();
        }

        public async Task<Document?> CheckDuplicateDocumentAsync(int? number, string notation, string epitome)
        {
            return await _context.Documents.FirstOrDefaultAsync(c => c.Number == number && c.Notation.ToLower() == notation.ToLower() && c.Epitome.ToLower() == epitome.ToLower() && c.IsDeleted == false);
        }

        public async Task<IEnumerable<Document>> QueryAsync(DocumentQuery query, bool trackChanges = false)
        {
            IQueryable<Document> documents = _context.Documents.Include(c=>c.DocumentType).Where(c => c.IsDeleted == false);

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
                documents = documents.Where(c => c.Notation.Contains(query.SearchText));
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
