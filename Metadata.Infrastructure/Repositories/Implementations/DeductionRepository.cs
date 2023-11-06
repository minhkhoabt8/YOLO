using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.Support;
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
    public class DeductionRepository : GenericRepository<Deduction, YoloMetadataContext>, IDeductionRepository
    {
        public DeductionRepository(YoloMetadataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Deduction?>> GetAllDeductionsOfOwnerAsync(string ownerId)
        {
            return await _context.Deductions.Where(c => c.OwnerId == ownerId).ToListAsync();
        }
        public async Task<decimal> CaculateTotalDeductionOfOwnerAsync(string ownerId)
        {
            return await _context.Deductions.Where(c => c.OwnerId == ownerId).SumAsync(c=>c.DeductionPrice);
        }
        public async Task<IEnumerable<Deduction>> QueryAsync(DeductionQuery query, bool trackChanges = false)
        {
            IQueryable<Deduction> supports = _context.Deductions;

            if (!trackChanges)
            {
                supports = supports.AsNoTracking();
            }

            IEnumerable<Deduction> enumeratedAssetCompensation = supports.AsEnumerable();
            return await Task.FromResult(enumeratedAssetCompensation);
        }
    }
}
