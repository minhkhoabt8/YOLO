using DocumentFormat.OpenXml.InkML;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;
using System.Numerics;

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
        public async Task<IEnumerable<Owner>> GetAllOwner()
        {
            return await Task.FromResult(_context.Owners.Where(o => o.IsDeleted == false));
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
                owners = owners.Where(c => c.OwnerName.Contains(query.SearchText)); ;
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                owners = owners.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Owner> enumeratedOwner = owners.AsEnumerable();
            return await Task.FromResult(enumeratedOwner);
        }

        public async Task<IEnumerable<Owner>> GetOwnerInProjectThatNotInAnyPlanAsync(string projecId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.ProjectId == projecId && o.PlanId == null));
        }

        //get owner by code and is deleted status 
        public async Task<Owner?> FindByCodeAndIsDeletedStatus(string code)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.OwnerCode.ToLower() == code.ToLower() && x.IsDeleted == false);
        }

        //get owner by  only OwnerIdCode property and is delete status
        public async Task<Owner?> FindByOwnerIdCodeAsync(string iDcode)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.OwnerIdCode == iDcode && x.IsDeleted == false);
        }

        //get owner by  only ownertaxcode property and is delete status
        public async Task<Owner?> FindByTaxCodeAsync(string taxCode)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.OwnerTaxCode == taxCode && x.IsDeleted == false);
        }


       
       



    }
}
