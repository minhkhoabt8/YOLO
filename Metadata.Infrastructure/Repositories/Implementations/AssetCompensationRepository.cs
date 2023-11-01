using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
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
            return await _context.AssetCompensations.Include(c=>c.AttachFiles).Where(c => c.OwnerId == ownerId).ToListAsync();
        }

        public async Task<decimal> CaculateAssetCompensationOfOwnerAsync(string ownerId, AssetOnLandTypeEnum? assetType)
        {
            var totalCompensation = _context.AssetCompensations.Where(c => c.OwnerId == ownerId);

            decimal total = 0;

            if (assetType == null)
            {
                total = await totalCompensation.SumAsync(c => c.CompensationPrice);
            }
            else
            {
                total = await totalCompensation
                    .Where(c => c.CompensationType == assetType.ToString())
                    .SumAsync(c => c.CompensationPrice);
            }

            return total;
        }
    }
}
