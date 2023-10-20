using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using SharedLib.Infrastructure.Repositories.Interfaces;


namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IGCNLandInfoRepository :
        IAddAsync<GcnlandInfo>,
        IFindAsync<GcnlandInfo>,
        IGetAllAsync<GcnlandInfo>,
        IUpdate<GcnlandInfo>,
        IDelete<GcnlandInfo>,
        IQueryAsync<GcnlandInfo, GCNLandInfoQuery>
    {
        Task<IEnumerable<GcnlandInfo>> GetAllGcnLandInfosOfOwnerAsync(string ownerId);
    }
}
