﻿using DocumentFormat.OpenXml.InkML;
using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Owner>> QueryOwnersOfProjectAsync(string projectId, OwnerQuery query, bool trackChanges = false)
        {
            IQueryable<Owner> owners = _context.Owners
                .Include(o => o.Supports)
                .Include(o => o.Deductions)
                .Include(o => o.GcnlandInfos)
                .Include(o => o.AssetCompensations)
                .Include(o => o.AttachFiles)
                .Include(o=> o.Plan)
                .Where(e => e.IsDeleted == false && e.ProjectId == projectId);

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
                owners = owners.Where(c => c.OwnerName.Contains(query.SearchText) || c.OwnerCode.Contains(query.SearchText));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                owners = owners.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Owner> enumeratedOwner = owners.AsEnumerable();
            return await Task.FromResult(enumeratedOwner);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Owner>> GetOwnersOfProjectAsync(string projectId)
        {
            return await Task.FromResult(_context.Owners.Include(c => c.Plan).Where(o => o.ProjectId == projectId && o.IsDeleted == false));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Owner>> GetOwnersOfPlanAsync(string planId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.PlanId == planId && o.IsDeleted == false));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Owner>> GetAllOwner()
        {
            return await Task.FromResult(_context.Owners.Where(o => o.IsDeleted == false));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<int> GetTotalOwnerInPlanAsync(string planId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.PlanId == planId && o.IsDeleted == false).Count());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projecId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Owner>> GetOwnerInProjectThatNotInAnyPlanAsync(string projecId)
        {
            return await Task.FromResult(_context.Owners.Where(o => o.ProjectId == projecId && o.PlanId == null));
        }

        /// <summary>
        /// get owner by code and is deleted status 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Owner?> FindByCodeAndIsDeletedStatus(string code)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.OwnerCode.ToLower() == code.ToLower() && x.IsDeleted == false);
        }

        /// <summary>
        /// get owner by  only OwnerIdCode property and is delete status
        /// </summary>
        /// <param name="iDcode"></param>
        /// <returns></returns>
        public async Task<Owner?> FindByOwnerIdCodeAsync(string iDcode)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.OwnerIdCode == iDcode && x.IsDeleted == false);
        }

        /// <summary>
        /// get owner by  only ownertaxcode property and is delete status
        /// </summary>
        /// <param name="taxCode"></param>
        /// <returns></returns>
        public async Task<Owner?> FindByTaxCodeAsync(string taxCode)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.OwnerTaxCode == taxCode && x.IsDeleted == false);
        }

        /// <summary>
        /// api check duplicate id code, tax code in a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="ownerTaxCode"></param>
        /// <param name="ownerIdCode"></param>
        /// <returns></returns>
        public async Task<Owner?> CheckDuplicateOwnerAsync(string projectId, string? ownerTaxCode, string? ownerIdCode)
        {
            IQueryable<Owner> owners = _context.Owners
                    .Where(o => o.ProjectId == projectId && !o.IsDeleted);

            if (!string.IsNullOrEmpty(ownerTaxCode))
            {
                owners = owners.Where(x => x.OwnerTaxCode == ownerTaxCode);
            }

            if (!string.IsNullOrEmpty(ownerIdCode))
            {
                owners = owners.Where(x => x.OwnerIdCode == ownerIdCode);
            }

            var result = await owners.FirstOrDefaultAsync();

            return result;
        }

        public async Task<Owner?> FindByOwnerIdCodeInProjectAsync(string projectId, string iDcode)
        {
            return await _context.Owners
            .Where(o => o.ProjectId == projectId && o.OwnerIdCode == iDcode)
            .FirstOrDefaultAsync();
        }

        public async Task<Owner?> FindByTaxCodeInProjectAsync(string projectId, string taxCode)
        {
            return await _context.Owners
            .Where(o => o.ProjectId == projectId && o.OwnerTaxCode == taxCode)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Owner>> GetOwnersHaveLandResettlementInProjectAsync(string projectId)
        {
            return await _context.Owners
                .Where(o => o.IsDeleted == false && o.ProjectId == projectId)
                .Include(o => o.LandResettlements)
                    .Where(o => o.LandResettlements
                        .Any(lr => lr.ResettlementProjectId == projectId))
                .ToListAsync();
        }

        public async Task<int> GetTotalLandResettlementsOfOwnersInProjectAsync(string projectId)
        {
            var totalLandResettlements = await _context.Owners
                .Where(o => o.IsDeleted == false && o.ProjectId == projectId)
                .Include(o => o.LandResettlements)
                .SelectMany(o => o.LandResettlements)
                .CountAsync();

            return totalLandResettlements;
        }
    }
}
