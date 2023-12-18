    using DocumentFormat.OpenXml.InkML;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                total = await totalLandCompensationPrice.SumAsync(c => c.CompensationPrice ?? 0);
            }
            else
            {
                total = await totalLandCompensationPrice.SumAsync(c => (decimal?)c.WithdrawArea * c.CompensationRate * 0.01m * c.UnitPriceLandCost) ?? 0;
            }

            return total;
        }
        /// <summary>
        /// Caculate Total Land Recovery Area Of Owner
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<decimal> CaculateTotalLandRecoveryAreaOfOwnerAsync(string ownerId)
        {
            var totalLandRecoveryOfOwner = _context.MeasuredLandInfos.Where(c => c.OwnerId == ownerId);

            return await totalLandRecoveryOfOwner.SumAsync(c => c.WithdrawArea) ?? 0;

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
                measuredLandInfos = measuredLandInfos.Where(c => c.MeasuredPageNumber.Contains(query.SearchText));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                measuredLandInfos = measuredLandInfos.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<MeasuredLandInfo> enumeratedMeasuredLandInfos = measuredLandInfos.AsEnumerable();

            return await Task.FromResult(enumeratedMeasuredLandInfos);
        }

        public async Task<MeasuredLandInfo?> CheckDuplicateMeasuredLandInfo(string pageNumber, string plotNumber, string? landTypeId = null)
        {
            var query = _context.MeasuredLandInfos
                        .Where(c => c.MeasuredPageNumber == pageNumber && c.MeasuredPlotNumber == plotNumber && !c.IsDeleted);

            if (!landTypeId.IsNullOrEmpty())
            {
                query = query.Where(c => c.LandTypeId == landTypeId);
            }

            return await query.FirstOrDefaultAsync();

        }

        /// <summary>
        /// Check if there are other owners with the same MeasuredPlotNumber and MeasuredPlotAddress but different LandTypeId
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="measuredPlotNumber"></param>
        /// <param name="measuredPlotAddress"></param>
        /// <param name="landTypeId"></param>
        /// <returns></returns>
        public async Task<bool> HasDuplicateMeasuredPlotAsync(string ownerId, string measuredPlotNumber, string measuredPageNumber, string landTypeId)
        {
            var otherOwnersWithSamePlot = await _context.MeasuredLandInfos
                .Where(info => info.OwnerId != ownerId &&
                               info.MeasuredPlotNumber == measuredPlotNumber &&
                               info.MeasuredPageNumber == measuredPageNumber &&
                               info.LandTypeId != landTypeId)
                .AnyAsync();

            return otherOwnersWithSamePlot;
        }

        /// <summary>
        /// Check if there are other owners with the same MeasuredPlotNumber and MeasuredPlotAddress
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="measuredPlotNumber"></param>
        /// <param name="measuredPlotAddress"></param>
        /// <returns></returns>
        public async Task<bool> HasDuplicateMeasuredPlotAndAddressAsync(string ownerId, string measuredPlotNumber, string measuredPageNumber)
        {
            var otherOwnersWithSamePlotAndAddress = await _context.MeasuredLandInfos
                .Where(info => info.OwnerId != ownerId &&
                               info.MeasuredPlotNumber == measuredPlotNumber &&
                               info.MeasuredPageNumber == measuredPageNumber)
                .AnyAsync();

            return otherOwnersWithSamePlotAndAddress;
        }

    }
}
