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
    public class LandTypeRepository : GenericRepository<LandType , YoloMetadataContext> , ILandTypeRepository
    {
        public LandTypeRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<LandType?> FindByCodeAsync(string code)
        {
            return await _context.LandTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<LandType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.LandTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<LandType>?> GetAllDeletedLandTypes()
        {
            return await _context.LandTypes.Where(x => x.IsDeleted == true).ToListAsync();
        }
    }
}