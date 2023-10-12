using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IOrganizationTypeRepository : IGetAllAsync<OrganizationType>,
        IFindAsync<OrganizationType>,
        IAddAsync<OrganizationType>,
        IDelete<OrganizationType>
    {
        Task<OrganizationType?> FindByCodeAsync(string code);
    }
 
}
