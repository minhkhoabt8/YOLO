﻿using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.ResettlementProject;
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
    public class ResettlementProjectRepository : GenericRepository<ResettlementProject, YoloMetadataContext>, IResettlementProjectRepository
    {
        public ResettlementProjectRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ResettlementProject>> GetResettlementProjectsInProjectAsync(string projectId)
        {
            
            throw new NotImplementedException();
        }

        public async Task<ResettlementProject?> GetResettlementProjectInProjectAsync(string projectId)
        {
            return await Task.FromResult(_context.Projects
                .Include(c => c.ResettlementProject)
                .FirstOrDefault(p => p.ProjectId == projectId)?.ResettlementProject);
        }


        public async Task<IEnumerable<ResettlementProject>> QueryAsync(ResettlementProjectQuery query, bool trackChanges = false)
        {
            IQueryable<ResettlementProject> resettlementProjects = _context.ResettlementProjects.Include(p => p.LandResettlements).Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                resettlementProjects = resettlementProjects.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                resettlementProjects = resettlementProjects.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                resettlementProjects = resettlementProjects.Where(c => c.Name.Contains(query.SearchText)); ;
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                resettlementProjects = resettlementProjects.OrderByDynamic(query.OrderBy);
            }

            IEnumerable<ResettlementProject> enumeratedresettlementProjects = resettlementProjects.AsEnumerable();

            return await Task.FromResult(enumeratedresettlementProjects);
        }
        //FindByCode And IsDeletedStatus
        public async Task<ResettlementProject?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted)
        {
            return await _context.ResettlementProjects.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower() && x.IsDeleted == isDeleted);
        }
        //FindByName And IsDeletedStatus
        public async Task<ResettlementProject?> FindByNameAndIsDeletedStatus(string name, bool isDeleted)
        {
            return await _context.ResettlementProjects.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted == isDeleted);
        }
        public async Task<ResettlementProject?> CheckDuplicateResettlementProjectAsync(string code, string name)
        {
            return await _context.ResettlementProjects.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()&& x.Code.ToLower() == code.ToLower() && x.IsDeleted == false);
        }
    }
}
