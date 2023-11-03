using DocumentFormat.OpenXml.InkML;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class OwnerRepository : GenericRepository<Owner, YoloMetadataContext>, IOwnerRepository
    {
        public OwnerRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Owner>> GetOwnersOfProjectAsync(string projectId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.ProjectId == projectId && o.IsDeleted == false));
        }

        public async Task<IEnumerable<Owner>> GetOwnersOfPlanAsync(string planId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.PlanId == planId && o.IsDeleted == false));
        }

        public async Task<int> GetTotalOwnerInPlanAsync(string planId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.PlanId == planId && o.IsDeleted == false).Count());
        }

        public async Task<IEnumerable<Owner>> QueryAsync(OwnerQuery query, bool trackChanges = false)
        {
            IQueryable<Owner> owners = _context.Owners
                .Include(o=> o.Supports)
                .Include(o => o.Deductions)
                .Include(o=> o.GcnlandInfos)
                .Include(o=> o.AssetCompensations)
                .Include(o=> o.AttachFiles)
                .Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                owners = owners.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                owners = owners.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                owners = owners.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                owners = owners.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Owner> enumeratedOwner = owners.AsEnumerable();
            return await Task.FromResult(enumeratedOwner);
        }
    }
}
