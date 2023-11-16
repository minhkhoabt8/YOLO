﻿using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IResettlementProjectRepository : IAddAsync<ResettlementProject>,
        IGetAllAsync<ResettlementProject>,
        IFindAsync<ResettlementProject>,
        IUpdate<ResettlementProject>,
        IDelete<ResettlementProject>,
        IQueryAsync<ResettlementProject,ResettlementProjectQuery>
    {
        Task<IEnumerable<ResettlementProject>> GetResettlementProjectsInProjectAsync(string projectId);
    }
}
