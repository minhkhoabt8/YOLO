using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IAssetGroupRepository : IGetAllAsync<AssetGroup>,
        IFindAsync<AssetGroup>,
        IAddAsync<AssetGroup>,
        IDelete<AssetGroup>
    {
        Task<AssetGroup?> FindByCodeAsync(string code);
        Task<AssetGroup> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<IEnumerable<AssetGroup>?> GetAllActivedDeletedAssetGroup();
        Task<AssetGroup?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<AssetGroup>> QueryAsync(AssetGroupQuery query, bool trackChanges = false);


      /*  Task<AssetGroup?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted);
        Task<AssetGroup?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted);*/


    }
}
