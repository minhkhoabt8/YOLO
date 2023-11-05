using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.OrganizationType;
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
            IQueryable<OrganizationType> organizationTypes = _context.OrganizationTypes;

            if (!trackChanges)
            {
                organizationTypes = organizationTypes.AsNoTracking();
            }

            IEnumerable<OrganizationType> enumeratedOrganizationTypes = organizationTypes.AsEnumerable();
            return await Task.FromResult(enumeratedOrganizationTypes);
        }
    }
    
}
