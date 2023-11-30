using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IOwnerRepository :
        IFindAsync<Owner>,
        IAddAsync<Owner>,
        IUpdate<Owner>,
        IDelete<Owner>,
        IQueryAsync<Owner, OwnerQuery>
    {
        Task<IEnumerable<Owner>> GetOwnersOfProjectAsync(string projectId);
        Task<IEnumerable<Owner>> GetOwnersOfPlanAsync(string planId);
        Task<int> GetTotalOwnerInPlanAsync(string planId);
        Task<IEnumerable<Owner>> GetOwnerInProjectThatNotInAnyPlanAsync(string projecId);
        Task<IEnumerable<Owner>> GetAllOwner();
        Task<Owner?> FindByCodeAndIsDeletedStatus(string code);
        Task<Owner?> FindByOwnerIdCodeAsync(string iDcode);
        Task<Owner?> FindByTaxCodeAsync(string taxCode);
        Task<IEnumerable<Owner>> QueryOwnersOfProjectAsync(string projectId, OwnerQuery query, bool trackChanges = false);
    }
}
