﻿    using DocumentFormat.OpenXml.InkML;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
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
        /// <summary>
        /// If reCheck = true, re-caculate asset compensation, else get directly from field (UnitPriceLandCost)
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="reCheck"></param>
        /// <returns></returns>
        public async Task<decimal> CaculateTotalLandCompensationPriceOfOwnerAsync(string ownerId, bool? reCheck = false)
        {
            var totalLandCompensationPrice =  _context.MeasuredLandInfos.Include(c=>c.UnitPriceLand).Where(c => c.OwnerId == ownerId);

            decimal total = 0;

            if(reCheck == false)
            {
                total = await totalLandCompensationPrice.SumAsync(c => c.UnitPriceLandCost);
            }

            total = await totalLandCompensationPrice.SumAsync(c => c.WithdrawArea * c.CompensationRate * c.UnitPriceLandCost) ?? 0;
             
            return total;
        }

        public async Task<IEnumerable<MeasuredLandInfo>> QueryAsync(MeasuredLandInfoQuery query, bool trackChanges = false)
        {
            IQueryable<MeasuredLandInfo> measuredLandInfos = _context.MeasuredLandInfos
                .Include(c => c.AttachFiles)
                .Where(e => e.IsDeleted == false);

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
