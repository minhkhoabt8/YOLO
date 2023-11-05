using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class ProjectRepository : GenericRepository<Project, YoloMetadataContext>, IProjectRepository
    {
        public ProjectRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<Project?> GetProjectIncludePlanByPlanIdAsync(string planId)
        {
            return await _context.Projects
                .Include(p => p.Plans) // Include the Plans navigation property
                .Where(p => p.Plans.Any(plan => plan.PlanId == planId))
                .SingleOrDefaultAsync();
        }

        public async Task<Project?> GetProjectByPlandIdAsync(string planId)
        {
            return await _context.Projects
                .Where(p => p.Plans.Any(plan => plan.PlanId == planId))
                .SingleOrDefaultAsync();
        }

        public async Task<Project?> GetProjectByNameAsync(string projectName)
        {
            return await _context.Projects.Where(p => p.ProjectName == projectName).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Project>> QueryAsync(ProjectQuery query, bool trackChanges = false)
        {
            IQueryable<Project> projects = _context.Projects.Include(p => p.LandPositionInfos).Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                projects = projects.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                projects = projects.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                projects = projects.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                projects = projects.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Project> enumeratedProject = projects.AsEnumerable();
            return await Task.FromResult(enumeratedProject);
        }

        public async Task<IEnumerable<Project>> GetProjectsOfOwnerAsync(string ownerId)
        {
            return await _context.Projects.Where(p => p.ProjectCode == ownerId).ToListAsync();
        }
    }
}
