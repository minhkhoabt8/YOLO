using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
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
    public class UnitPriceAssetRepository : GenericRepository<UnitPriceAsset, YoloMetadataContext>, IUnitPriceAssetRepository
    {
        public UnitPriceAssetRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UnitPriceAsset>> GetUnitPriceAssetsOfProjectAsync(string projectId)
        {
            return await Task.FromResult(_context.UnitPriceAssets.Where(upa => upa.PriceAppliedCode.Projects.Any(p => p.ProjectId == projectId)));
        }

        public async Task<IEnumerable<UnitPriceAsset>> QueryAsync(UnitPriceAssetQuery query, bool trackChanges = false)
        {
            IQueryable<UnitPriceAsset> unitPriceAssets = _context.UnitPriceAssets.Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                unitPriceAssets = unitPriceAssets.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                unitPriceAssets = unitPriceAssets.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                unitPriceAssets = unitPriceAssets.Where(c => c.AssetName.Contains(query.SearchText)); ;
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                unitPriceAssets = unitPriceAssets.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<UnitPriceAsset> enumeratedUnitPriceAssets = unitPriceAssets.AsEnumerable();
            return await Task.FromResult(enumeratedUnitPriceAssets);
        }
    }
}
