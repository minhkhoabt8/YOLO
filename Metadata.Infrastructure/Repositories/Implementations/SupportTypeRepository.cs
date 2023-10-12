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
    public class SupportTypeRepository : GenericRepository<SupportType, YoloMetadataContext>, ISupportTypeRepository
    {
        public SupportTypeRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<SupportType?> FindByCodeAsync(string code)
        {
            return await _context.SupportTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());

        }


    }
}
