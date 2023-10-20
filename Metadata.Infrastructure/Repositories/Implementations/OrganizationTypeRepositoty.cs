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

        public async Task<IEnumerable<OrganizationType>?> GetAllDeletedOrganizationTypes()
        {
            return await _context.OrganizationTypes.Where(x => x.IsDeleted == true).ToListAsync();
        }
    }
    
}
