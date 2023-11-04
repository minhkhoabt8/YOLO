    using DocumentFormat.OpenXml.InkML;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class MeasuredLandInfoRepository : GenericRepository<MeasuredLandInfo, YoloMetadataContext>, IMeasuredLandInfoRepository
    {
        public MeasuredLandInfoRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MeasuredLandInfo>> GetAllMeasuredLandInfosOfOwnerAsync(string ownerId)
        {
            return await _context.MeasuredLandInfos.Include(c => c.AttachFiles).Where(c => c.OwnerId == ownerId).ToListAsync();
        }

        public async Task<decimal?> CaculateTotalUnitPriceLandOfOwnerAsync(string ownerId)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<MeasuredLandInfo>> QueryAsync(MeasuredLandInfoQuery query, bool trackChanges = false)
        {
            IQueryable<MeasuredLandInfo> measuredLandInfos = _context.MeasuredLandInfos.Include(c => c.AttachFiles);

            if (!trackChanges)
            {
                measuredLandInfos = measuredLandInfos.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                measuredLandInfos = measuredLandInfos.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                measuredLandInfos = measuredLandInfos.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                measuredLandInfos = measuredLandInfos.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<MeasuredLandInfo> enumeratedMeasuredLandInfos = measuredLandInfos.AsEnumerable();

            return await Task.FromResult(enumeratedMeasuredLandInfos);
        }
    }
}
