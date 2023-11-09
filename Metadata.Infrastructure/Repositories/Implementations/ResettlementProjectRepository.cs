using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Repositories.Implementations;
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
    }
}
