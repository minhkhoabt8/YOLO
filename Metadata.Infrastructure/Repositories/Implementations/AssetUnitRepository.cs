using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
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

        public async Task<AssetUnit?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.AssetUnits.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<AssetUnit>?> GetActivedAssetUnitAsync()
        {
            return await _context.AssetUnits.Where(x => x.IsDeleted == false).ToListAsync();
        }

        
        public async Task<IEnumerable<AssetUnit>> QueryAsync(AssetUnitQuery query, bool trackChanges = false)
        {
            IQueryable<AssetUnit> assetUnits = _context.AssetUnits
                  .Where(c => c.IsDeleted == false); ;

            if (!trackChanges)
            {
                assetUnits = assetUnits.AsNoTracking();
            }

            IEnumerable<AssetUnit> enumeratedAssetUnits = assetUnits.AsEnumerable();
            return await Task.FromResult(enumeratedAssetUnits);
        }

        public async Task<AssetUnit?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted)
        {
            var check = await _context.AssetUnits.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.AssetUnitId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }

        public async Task<AssetUnit?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted)
        {
            var check = await _context.AssetUnits.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.AssetUnitId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }
    }
    
}
