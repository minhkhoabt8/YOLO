using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
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
    public class LandPositionInfoRepository : GenericRepository<LandPositionInfo, YoloMetadataContext>, ILandPositionInfoRepository
    {
        public LandPositionInfoRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LandPositionInfo>> QueryAsync(LandPositionInfoQuery query, bool trackChanges = false)
        {
            IQueryable<LandPositionInfo> landPositionInfos = _context.LandPositionInfos;

            if (!trackChanges)
            {
                landPositionInfos = landPositionInfos.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                landPositionInfos = landPositionInfos.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                landPositionInfos = landPositionInfos.Where(c => c.LocationName.Contains(query.SearchText));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                landPositionInfos = landPositionInfos.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<LandPositionInfo> enumeratedLandPositionInfos = landPositionInfos.AsEnumerable();
            return await Task.FromResult(enumeratedLandPositionInfos);
        }
    }
}
