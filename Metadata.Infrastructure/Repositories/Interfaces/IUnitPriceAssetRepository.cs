using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IUnitPriceAssetRepository : IGetAllAsync<UnitPriceAsset>,
        IFindAsync<UnitPriceAsset>,
        IAddAsync<UnitPriceAsset>,
        IDelete<UnitPriceAsset>,
        IQueryAsync<UnitPriceAsset, UnitPriceAssetQuery>
    {
        Task<IEnumerable<UnitPriceAsset>> GetUnitPriceAssetsOfProjectAsync(string projectId);
    }
}
