using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class LandGroupRepository : GenericRepository<LandGroup , YoloMetadataContext> , ILandGroupRepository
    {
        public LandGroupRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<LandGroup?> FindByCodeAsync(string code)
        {
            return await _context.LandGroups.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());

        }

        public async Task<LandGroup?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.LandGroups.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<LandGroup?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.LandGroups.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<LandGroup>?> GetAllActivedLandGroups()
        {
            return await _context.LandGroups.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<LandGroup>> QueryAsync (LandGroupQuery query , bool trackChanges = false)
        {
            IQueryable<LandGroup> landGroups = _context.LandGroups.Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                landGroups = landGroups.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                landGroups = landGroups.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                landGroups = landGroups.Where(c => c.Name.Contains(query.SearchText)); ;
            }
            //search by code
            if (!string.IsNullOrWhiteSpace(query.SearchByNames))
            {
                landGroups = landGroups.Where(c => c.Code.Contains(query.SearchByNames)); ;
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                landGroups = landGroups.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<LandGroup> enumeratedLandGroups = landGroups.AsEnumerable();
            return await Task.FromResult(enumeratedLandGroups);
        }

        public async Task<LandGroup?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted)
        {
            var check = await _context.LandGroups.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.LandGroupId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }

        public async Task<LandGroup?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted)
        {
            var check = await _context.LandGroups.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.LandGroupId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }
    }
    
}
