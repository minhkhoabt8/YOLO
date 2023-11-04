using Metadata.Core.Data;
using Metadata.Core.Entities;
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
    public class AssetGroupRepository : GenericRepository<AssetGroup , YoloMetadataContext> , IAssetGroupRepository
    {
        public AssetGroupRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<AssetGroup?> FindByCodeAsync(string code)
        {
            return await _context.AssetGroups.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());

        }

        public async Task<AssetGroup?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.AssetGroups.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<AssetGroup?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.AssetGroups.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }


        public async Task<IEnumerable<AssetGroup>?> GetAllActivedDeletedAssetGroup()
        {
            return await _context.AssetGroups.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<AssetGroup>> QueryAsync (AssetGroupQuery query , bool trackChanges = false)
        {
            IQueryable<AssetGroup> assetGroups = _context.AssetGroups;

            if (!trackChanges)
            {
                assetGroups = assetGroups.AsNoTracking();
            }

            IEnumerable<AssetGroup> enumeratedAssetGroups = assetGroups.AsEnumerable();
            return await Task.FromResult(enumeratedAssetGroups);
        }
        
    }
}
