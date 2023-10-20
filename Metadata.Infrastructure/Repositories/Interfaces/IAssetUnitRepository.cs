using Metadata.Core.Entities;
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
        Task<IEnumerable<AssetUnit>?> GetAllDeletedAssetUnit();
    }
}
