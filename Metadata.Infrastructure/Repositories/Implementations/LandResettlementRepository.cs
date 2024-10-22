﻿using DocumentFormat.OpenXml.InkML;
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

        public async Task<decimal> CaculateTotalLandPricesOfOwnerAsync(string ownerId)
        {
            return await _context.LandResettlements.Where(c => c.OwnerId == ownerId).SumAsync(c => c.TotalLandPrice ?? 0);
        }

        public async Task<LandResettlement?> CheckDuplicateLandResettlement(string pageNumber, string plotNumber)
        {
            var query = _context.LandResettlements
                        .Where(c => c.PageNumber == pageNumber && c.PlotNumber == plotNumber);

            return await query.FirstOrDefaultAsync();

        }

        public async Task<decimal> CalculateOwnerTotalLandResettlementPriceInPlanAsync(string planId)
        {
            return await Task.FromResult( _context.Owners
                .Where(owner => owner.PlanId == planId && owner.IsDeleted == false)
                .SelectMany(owner => _context.LandResettlements
                    .Where(lr => lr.OwnerId == owner.OwnerId))
                .Sum(lr => lr.TotalLandPrice ?? 0));
        }

    }
}
