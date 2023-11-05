using Metadata.Core.Entities;
using Metadata.Core.Enums;
using SharedLib.Infrastructure.Repositories.Interfaces;


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
        Task<decimal> CaculateTotalAssetCompensationOfOwnerAsync(string ownerId, AssetOnLandTypeEnum? assetType, bool? reCheck = false);
    }
}
