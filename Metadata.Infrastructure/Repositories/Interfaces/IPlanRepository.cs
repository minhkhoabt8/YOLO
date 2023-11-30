using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IPlanRepository :
         IFindAsync<Plan>,
        IAddAsync<Plan>,
        IUpdate<Plan>,
        IDelete<Plan>,
        IQueryAsync<Plan, PlanQuery>
    {
        Task <IEnumerable<Plan>> QueryPlansOfProjectAsync(string projectId, PlanQuery query, bool trackChanges = false);
        Task<IEnumerable<Plan>> GetPlansOfProjectAsync(string projectId);
        Task<Plan?> GetPlanByPlanCodeAsync(string planCode);
        Task<IEnumerable<Plan>> QueryPlanOfCreatorAsync(PlanQuery query, string creatorName, PlanStatusEnum? planStatus, bool trackChanges = false);
    }
}
