using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IAssetCompensationRepository :
        IAddAsync<AssetCompensation>,
        IFindAsync<AssetCompensation>,
        IGetAllAsync<AssetCompensation>,
        IUpdate<AssetCompensation>,
        IDelete<AssetCompensation>
    {
        Task<IEnumerable<AssetCompensation?>> GetAllAssetCompensationsOfOwnerAsync(string ownerId);
    }
}
