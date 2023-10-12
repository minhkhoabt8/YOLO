using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AuditTrail;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;


namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class AuditTrailRepository : GenericRepository<AuditTrail, YoloMetadataContext>, IAuditTrailRepository
    {
        public AuditTrailRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditTrail>> QueryAsync(AuditTrailQuery query, bool trackChanges = false)
        {
            IQueryable<AuditTrail> audits = _context.AuditTrails;

            if (!trackChanges)
            {
                audits = audits.AsNoTracking();
            }
           
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                audits = audits.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                audits = audits.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<AuditTrail> enumeratedAudits = audits.AsEnumerable();
            return await Task.FromResult(enumeratedAudits);
        }
    }
}
