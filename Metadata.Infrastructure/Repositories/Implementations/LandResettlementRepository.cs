using Amazon.S3.Model;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<decimal> CaculateTotalLandPricesOfOwnerAsync(string ownerId)
        {
            return await _context.LandResettlements.Where(c => c.OwnerId == ownerId).SumAsync(c => c.TotalLandPrice);
        }
    }
}
