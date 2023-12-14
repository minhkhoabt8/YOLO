using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;


namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class LandResettlementRepository : GenericRepository<LandResettlement, YoloMetadataContext>, ILandResettlementRepository
    {
        public LandResettlementRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LandResettlement>> GetLandResettlementsOfOwnerIncludeResettlementProjectAsync(string ownerId)
        {
            return await Task.FromResult(_context.LandResettlements.Include(lr=>lr.ResettlementProject).Where(lr => lr.OwnerId == ownerId));
        }

        public async Task<IEnumerable<LandResettlement>> GetLandResettlementsOfResettlementProjectIncludeOwnerAsync(string resettlementProjectId)
        {
            return await Task.FromResult(_context.LandResettlements.Include(lr => lr.Owner).Where(lr => lr.ResettlementProjectId == resettlementProjectId));
        }

        public async Task<LandResettlement?> CheckDuplicateLandResettlement(string pageNumber, string plotNumber)
        {
            var query = _context.LandResettlements
                        .Where(c => c.PageNumber == pageNumber && c.PlotNumber == plotNumber);

            return await query.FirstOrDefaultAsync();

        }


    }
}
