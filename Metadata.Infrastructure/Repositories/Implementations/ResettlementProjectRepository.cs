using Metadata.Core.Data;
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
            return await Task.FromResult(_context.ResettlementProjects.Where(c => c.ProjectId == projectId));
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
                resettlementProjects = resettlementProjects.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                resettlementProjects = resettlementProjects.OrderByDynamic(query.OrderBy);
            }

            IEnumerable<ResettlementProject> enumeratedresettlementProjects = resettlementProjects.AsEnumerable();

            return await Task.FromResult(enumeratedresettlementProjects);
        }
    }
}
