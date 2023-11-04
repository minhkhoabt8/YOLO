using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandGroup;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ILandGroupRepository : IGetAllAsync<LandGroup>,
        IFindAsync<LandGroup>,
        IAddAsync<LandGroup>,
        IDelete<LandGroup>
    {
        Task<LandGroup?> FindByCodeAsync(string code);
        Task<LandGroup?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<LandGroup?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<LandGroup>?> GetAllActivedLandGroups();
        Task<IEnumerable<LandGroup>> QueryAsync(LandGroupQuery query, bool trackChanges = false);
    }
}
