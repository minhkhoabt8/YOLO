using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using SharedLib.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IUnitPriceLandRepository : IGetAllAsync<UnitPriceLand>,
        IFindAsync<UnitPriceLand>,
        IAddAsync<UnitPriceLand>,
        IDelete<UnitPriceLand>,
        IQueryAsync<UnitPriceLand, UnitPriceLandQuery>
    {
        Task<IEnumerable<UnitPriceLand>> GetUnitPriceLandsOfProjectAsync(string projectId);
        Task<IEnumerable<UnitPriceLand>> QueryUnitPriceLandOfProjectAsync(string projectId, UnitPriceLandQuery query, bool trackChanges = false);
    }
}
