using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
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
    public class PlanRepository : GenericRepository<Plan, YoloMetadataContext>, IPlanRepository
    {
        public PlanRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Plan>> GetPlansOfProjectAsync(string projectId)
        {
            return await Task.FromResult(_context.Plans.Include(c =>c .AttachFiles).Include(c => c.Owners).Where(c => c.ProjectId == projectId));
        }
        //get plan by plan code
        public async Task<Plan?> GetPlanByPlanCodeAsync(string planCode)
        {
            return await _context.Plans.Where(p => p.PlanCode == planCode && p.IsDeleted==false).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Plan>> QueryAsync(PlanQuery query, bool trackChanges = false)
        {
            IQueryable <Plan> plans = _context.Plans.Where(e => e.IsDeleted == false);

            if (!trackChanges)
            {
                plans = plans.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                plans = plans.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                plans = plans.Where(c => c.PlanCode.Contains(query.SearchText));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                plans = plans.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Plan> enumeratedPlan = plans.AsEnumerable();

            return await Task.FromResult(enumeratedPlan);
        }


        public async Task<IEnumerable<Plan>> QueryPlanOfCreatorAsync(PlanQuery query, string creatorName, PlanStatusEnum? planStatus, bool trackChanges = false)
        {
            IQueryable<Plan> plans = _context.Plans.Where(e => e.PlanCreatedBy == creatorName && e.IsDeleted == false);

            
            if (!trackChanges)
            {
                plans = plans.AsNoTracking();
            }
            if (planStatus.HasValue)
            {
                plans = plans.Where(e => e.PlanStatus == planStatus.ToString());
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                plans = plans.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                plans = plans.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                plans = plans.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Plan> enumeratedPlan = plans.AsEnumerable();

            return await Task.FromResult(enumeratedPlan);
        }

    }
}
