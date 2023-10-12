﻿using Metadata.Core.Data;
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
    public class AssetGroupRepository : GenericRepository<AssetGroup , YoloMetadataContext> , IAssetGroupRepository
    {
        public AssetGroupRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<AssetGroup?> FindByCodeAsync(string code)
        {
            return await _context.AssetGroups.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());

        }
    }
}
