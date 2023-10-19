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
    public class DeductionTypeRepository : GenericRepository<DeductionType , YoloMetadataContext> , IDeductionTypeRepository
    {
        public DeductionTypeRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<DeductionType?> FindByCodeAsync(string code)
        {
            return await _context.DeductionTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<DeductionType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.DeductionTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<DeductionType>?> GetAllDeletedDeductionTypes()
        {
            return await _context.DeductionTypes.Where(x => x.IsDeleted == true).ToListAsync();
        }
    }
}
