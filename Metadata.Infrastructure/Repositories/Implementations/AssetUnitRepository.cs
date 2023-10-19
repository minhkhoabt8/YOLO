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
    public class AssetUnitRepository : GenericRepository<AssetUnit , YoloMetadataContext> , IAssetUnitRepository
    {
        public AssetUnitRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<AssetUnit?> FindByCodeAsync(string code)
        {
            return await _context.AssetUnits.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<AssetUnit?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.AssetUnits.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<AssetUnit>?> GetAllDeletedAssetUnit()
        {
            return await _context.AssetUnits.Where(x => x.IsDeleted == true).ToListAsync();
        }
    }
    
}
