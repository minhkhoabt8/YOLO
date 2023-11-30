using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.OrganizationType;
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
    public class OrganizationTypeRepositoty : GenericRepository<OrganizationType , YoloMetadataContext> , IOrganizationTypeRepository
    {
        public OrganizationTypeRepositoty(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<OrganizationType?> FindByCodeAsync(string code)
        {
            return await _context.OrganizationTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());

        }

        public async Task<OrganizationType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.OrganizationTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }
        public async Task<OrganizationType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.OrganizationTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<OrganizationType>?> GetAllActivedOrganizationTypes()
        {
            return await _context.OrganizationTypes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<OrganizationType>> QueryAsync (OrganizationTypeQuery query , bool trackChanges = false)
        {
            IQueryable<OrganizationType> organizationTypes = _context.OrganizationTypes.Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                organizationTypes = organizationTypes.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                organizationTypes = organizationTypes.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                organizationTypes = organizationTypes.Where(c => c.Name.Contains(query.SearchText)); ;
            }
            //search by code
            if (!string.IsNullOrWhiteSpace(query.SearchByNames))
            {
                organizationTypes = organizationTypes.Where(c => c.Code.Contains(query.SearchByNames)); ;
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                organizationTypes = organizationTypes.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<OrganizationType> enumeratedOrganizationTypes = organizationTypes.AsEnumerable();
            return await Task.FromResult(enumeratedOrganizationTypes);
        }

        public async Task<OrganizationType?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted)
        {
            var check = await _context.OrganizationTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.OrganizationTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }

        public async Task<OrganizationType?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted)
        {
            var check = await _context.OrganizationTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.OrganizationTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }
    }
    
}
