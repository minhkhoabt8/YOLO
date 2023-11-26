using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class GCNLandInfoRepository : GenericRepository<GcnlandInfo, YoloMetadataContext>, IGCNLandInfoRepository
    {
        public GCNLandInfoRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GcnlandInfo>> GetAllGcnLandInfosOfOwnerAsync(string ownerId)
        {
            return await _context.GcnlandInfos.Include(c => c.AttachFiles).Where(c => c.OwnerId == ownerId).ToListAsync();
        }


        public async Task<IEnumerable<GcnlandInfo>> QueryAsync(GCNLandInfoQuery query, bool trackChanges = false)
        {
            IQueryable<GcnlandInfo> gcnLandInfos = _context.GcnlandInfos
                .Include(c => c.AttachFiles)
                .Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                gcnLandInfos = gcnLandInfos.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                gcnLandInfos = gcnLandInfos.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                gcnLandInfos = gcnLandInfos.Where(c => c.GcnPageNumber.Contains(query.SearchText));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                gcnLandInfos = gcnLandInfos.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<GcnlandInfo> enumeratedGcnLandInfos = gcnLandInfos.AsEnumerable();

            return await Task.FromResult(enumeratedGcnLandInfos);
        }
    }
}
