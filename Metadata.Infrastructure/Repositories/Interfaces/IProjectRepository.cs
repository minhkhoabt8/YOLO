using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Project;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IProjectRepository :
        IGetAllAsync<Project>,
        IFindAsync<Project>,
        IAddAsync<Project>,
        IUpdate<Project>,
        IDelete<Project>,
        IQueryAsync<Project, ProjectQuery>
    {
    }
}
