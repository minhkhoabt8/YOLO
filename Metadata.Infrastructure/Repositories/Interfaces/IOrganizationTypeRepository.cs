using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.OrganizationType;
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
        Task<OrganizationType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<OrganizationType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<OrganizationType>?> GetAllActivedOrganizationTypes();
        Task<IEnumerable<OrganizationType>> QueryAsync(OrganizationTypeQuery query, bool trackChanges = false);

        Task<OrganizationType?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted);
        Task<OrganizationType?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted);
    }
 
}
