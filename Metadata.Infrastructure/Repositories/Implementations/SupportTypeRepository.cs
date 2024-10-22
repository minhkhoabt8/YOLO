﻿using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.SupportType;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
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

        public async Task<SupportType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.SupportTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<SupportType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.SupportTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<SupportType>?> GetAllActivedSupportType()
        {
            return await _context.SupportTypes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<SupportType>> QueryAsync(SupportTypeQuery query, bool trackChanges = false)
        {
            IQueryable<SupportType> supportTypes = _context.SupportTypes.Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                supportTypes = supportTypes.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                supportTypes = supportTypes.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                supportTypes = supportTypes.Where(c => c.Name.Contains(query.SearchText)); ;
            }
            //search by code
            if (!string.IsNullOrWhiteSpace(query.SearchByNames))
            {
                supportTypes = supportTypes.Where(c => c.Code.Contains(query.SearchByNames)); ;
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                supportTypes = supportTypes.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<SupportType> enumeratedSupportTypes = supportTypes.AsEnumerable();
            return await Task.FromResult(enumeratedSupportTypes);
        }

        public async Task<SupportType?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted)
        {
            var check = await _context.SupportTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.SupportTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }

        public async Task<SupportType?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted)
        {
            var check = await _context.SupportTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.SupportTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }
    }
}
