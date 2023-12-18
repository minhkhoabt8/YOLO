using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using SharedLib.Infrastructure.Repositories.Interfaces;


namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IMeasuredLandInfoRepository :
        IAddAsync<MeasuredLandInfo>,
        IFindAsync<MeasuredLandInfo>,
        IGetAllAsync<MeasuredLandInfo>,
        IUpdate<MeasuredLandInfo>,
        IDelete<MeasuredLandInfo>,
        IQueryAsync<MeasuredLandInfo, MeasuredLandInfoQuery>
    {
        Task<IEnumerable<MeasuredLandInfo>> GetAllMeasuredLandInfosOfOwnerAsync(string ownerId);
        Task<decimal> CaculateTotalLandCompensationPriceOfOwnerAsync(string ownerId, bool? reCheck = false);
        Task<decimal> CaculateTotalLandRecoveryAreaOfOwnerAsync(string ownerId);
        Task<MeasuredLandInfo?> CheckDuplicateMeasuredLandInfo(string pageNumber, string plotNumber, string? landTypeId = null);
        Task<bool> HasDuplicateMeasuredPlotAsync(string ownerId, string measuredPlotNumber, string measuredPageNumber, string landTypeId);
        Task<bool> HasDuplicateMeasuredPlotAndAddressAsync(string ownerId, string measuredPlotNumber, string measuredPageNumber);
    }
}
