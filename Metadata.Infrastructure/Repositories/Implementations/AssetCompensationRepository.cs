using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.AssetGroup;
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
    public class AssetCompensationRepository : GenericRepository<AssetCompensation, YoloMetadataContext>, IAssetCompensationRepository
    {
        public AssetCompensationRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AssetCompensation?>> GetAllAssetCompensationsOfOwnerAsync(string ownerId)
        {
            return await _context.AssetCompensations.Where(c => c.OwnerId == ownerId && c.IsDeleted == false).ToListAsync();
        }


        public async Task<IEnumerable<AssetCompensation>> QueryAsync(AssetCompensationQuery query, bool trackChanges = false)
        {
            IQueryable<AssetCompensation> AssetCompensation = _context.AssetCompensations
                .Where(c=>c.IsDeleted == false);

            if (!trackChanges)
            {
                AssetCompensation = AssetCompensation.AsNoTracking();
            }

            IEnumerable<AssetCompensation> enumeratedAssetCompensation = AssetCompensation.AsEnumerable();
            return await Task.FromResult(enumeratedAssetCompensation);
        }

        /// <summary>
        /// If reCheck = true, re-caculate asset compensation, else get directly from field (CompensationPrice)
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="assetType"></param>
        /// <param name="reCheck"></param>
        /// <returns></returns>
        public async Task<decimal> CaculateTotalAssetCompensationOfOwnerAsync(string ownerId, AssetOnLandTypeEnum? assetType, bool? reCheck = false)
        {
            var totalAssetCompensation = _context.AssetCompensations.Include(c=>c.UnitPriceAsset).Where(c => c.OwnerId == ownerId);

            decimal total = 0;

            if (assetType == null && reCheck == false)
            {
                total = await totalAssetCompensation.SumAsync(c => c.CompensationPrice);
            }
            if (assetType == null && reCheck == true)
            {
                total = await totalAssetCompensation.SumAsync(c => c.QuantityArea * c.CompensationRate * c.UnitPriceAsset.AssetPrice);
            }
            if (assetType != null && reCheck == false)
            {
                total = await totalAssetCompensation
                    .Where(c => c.CompensationType == assetType.ToString())
                    .SumAsync(c => c.CompensationPrice);
            }
            if (assetType != null && reCheck == true)
            {
                total = await totalAssetCompensation
                    .Where(c => c.CompensationType == assetType.ToString())
                    .SumAsync(c => c.QuantityArea * c.CompensationRate * c.UnitPriceAsset.AssetPrice);
            }

            return total;
        }
    }
}
