using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class SupportRepository : GenericRepository<Support, YoloMetadataContext>, ISupportRepository
    {
        public SupportRepository(YoloMetadataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Support?>> GetAllSupportsOfOwnerAsync(string ownerId)
        {
            return await _context.Supports.Where(c => c.OwnerId == ownerId).ToListAsync();
        }
        public async Task<decimal> CaculateTotalSupportOfOwnerAsync(string ownerId)
        {
            return await _context.Supports.Where(c => c.OwnerId == ownerId).SumAsync(c => c.SupportPrice);
        }

        public async Task<IEnumerable<Support>> QueryAsync(SupportQuery query, bool trackChanges = false)
        {
            IQueryable<Support> supports = _context.Supports;

            if (!trackChanges)
            {
                supports = supports.AsNoTracking();
            }

            IEnumerable<Support> enumeratedAssetCompensation = supports.AsEnumerable();
            return await Task.FromResult(enumeratedAssetCompensation);
        }
    }
}
