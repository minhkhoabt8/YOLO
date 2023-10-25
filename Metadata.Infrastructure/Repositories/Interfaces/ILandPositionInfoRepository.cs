using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;


namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ILandPositionInfoRepository :
        IAddAsync<LandPositionInfo>,
        IUpdate<LandPositionInfo>,
        IGetAllAsync<LandPositionInfo>,
        IFindAsync<LandPositionInfo>,
        IDelete<LandPositionInfo>
    {
    }
}
