using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class PriceAppliedCodeRepository : GenericRepository<PriceAppliedCode, YoloMetadataContext>, IPriceAppliedCodeRepository
    {
        public PriceAppliedCodeRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<PriceAppliedCode?> GetPriceAppliedCodeByCodeAsync(string code)
        {
            return await _context.PriceAppliedCodes.Include(c=>c.UnitPriceAssets).Where(c=>c.UnitPriceCode == code && c.IsDeleted == false).FirstOrDefaultAsync();
        }

        //get UnitPriceCode in PriceAppliedCode through price_applied_code_id 
       public async Task<PriceAppliedCode?> GetUnitPriceCodeByProjectAsync(string priceAppliedCodeID)
       {
            return await _context.PriceAppliedCodes.Include(c => c.Projects).Where(c => c.PriceAppliedCodeId == priceAppliedCodeID).FirstOrDefaultAsync();
       }
       

        public async Task<IEnumerable<PriceAppliedCode>> QueryAsync(PriceAppliedCodeQuery query, bool trackChanges = false)
        {
            IQueryable<PriceAppliedCode> priceAppliedCodes = _context.PriceAppliedCodes
                //.Include(c=>c.UnitPriceAssets)
                .Where(c => c.IsDeleted == false);

            if (!trackChanges)
            {
                priceAppliedCodes = priceAppliedCodes.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                priceAppliedCodes = priceAppliedCodes.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                priceAppliedCodes = priceAppliedCodes.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                priceAppliedCodes = priceAppliedCodes.OrderByDynamic(query.OrderBy);
            }

            if(query.IsExpired == true)
            {
                priceAppliedCodes = priceAppliedCodes.Where(c => c.ExpriredTime >= DateTime.UtcNow);
            }

            IEnumerable<PriceAppliedCode> enumeratedPriceAppliedCodes = priceAppliedCodes.AsEnumerable();

            return await Task.FromResult(enumeratedPriceAppliedCodes);
        }
    }
}
