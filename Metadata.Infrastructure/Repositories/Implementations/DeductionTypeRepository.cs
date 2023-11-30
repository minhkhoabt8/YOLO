using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DeductionType;
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
        public async Task<DeductionType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.DeductionTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }
        public async Task<IEnumerable<DeductionType>?> GetActivedDeductionTypes()
        {
            return await _context.DeductionTypes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<DeductionType>> QueryAsync(DeductionTypeQuery query, bool trackChanges = false)
        {
            IQueryable<DeductionType> deductionTypes = _context.DeductionTypes.Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                deductionTypes = deductionTypes.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                deductionTypes = deductionTypes.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                deductionTypes = deductionTypes.Where(c => c.Name.Contains(query.SearchText)); ;
            }
            //search by code
            if (!string.IsNullOrWhiteSpace(query.SearchByNames))
            {
                deductionTypes = deductionTypes.Where(c => c.Code.Contains(query.SearchByNames)); ;
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                deductionTypes = deductionTypes.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<DeductionType> enumeratedDeductionTypes = deductionTypes.AsEnumerable();
            return await Task.FromResult(enumeratedDeductionTypes);
        }

        public async Task<DeductionType?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted)
        {
            var check = await _context.DeductionTypes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.DeductionTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }

        public async Task<DeductionType?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted)
        {
            var check = await _context.DeductionTypes.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.DeductionTypeId.ToLower() != id.ToLower() && x.IsDeleted == isDeleted);
            return check;
        }
    }
}
