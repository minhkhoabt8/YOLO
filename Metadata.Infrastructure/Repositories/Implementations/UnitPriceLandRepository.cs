using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class UnitPriceLandRepository : GenericRepository<UnitPriceLand, YoloMetadataContext>, IUnitPriceLandRepository
    {
        public UnitPriceLandRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UnitPriceLand>> GetUnitPriceLandsOfProjectAsync(string projectId)
        {
            return  await Task.FromResult(_context.UnitPriceLands.Include(c=>c.LandType).Where(c => c.ProjectId == projectId));
        }

        public async Task<IEnumerable<UnitPriceLand>> QueryAsync(UnitPriceLandQuery query, bool trackChanges = false)
        {
            IQueryable<UnitPriceLand> unitPriceLands = _context.UnitPriceLands.Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                unitPriceLands = unitPriceLands.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                unitPriceLands = unitPriceLands.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                unitPriceLands = unitPriceLands.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                unitPriceLands = unitPriceLands.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<UnitPriceLand> enumeratedUnitPriceLands = unitPriceLands.AsEnumerable();
            return await Task.FromResult(enumeratedUnitPriceLands);
        }
    }
    }
    
