using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IAssetUnitRepository : IGetAllAsync<AssetUnit>,
        IFindAsync<AssetUnit>,
        IAddAsync<AssetUnit>,
        IDelete<AssetUnit>
    {
        Task<AssetUnit?> FindByCodeAsync(string code);
        Task<AssetUnit?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<IEnumerable<AssetUnit>?> GetActivedAssetUnitAsync();
        Task<AssetUnit?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<AssetUnit>> QueryAsync(AssetUnitQuery query, bool trackChanges = false);
        Task<AssetUnit?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted);
        Task<AssetUnit?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted);


    }
}
