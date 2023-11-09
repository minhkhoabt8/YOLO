using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IUnitPriceAssetService
    {
        Task<PaginatedResponse<UnitPriceAssetReadDTO>> UnitPriceAssetQueryAsync(UnitPriceAssetQuery query);
        Task<UnitPriceAssetReadDTO> GetUnitPriceAssetAsync(string unitPriceAssetId);
        Task<UnitPriceAssetReadDTO> CreateUnitPriceAssetAsync(UnitPriceAssetWriteDTO dto);
        Task<UnitPriceAssetReadDTO> UpdateUnitPriceAssetAsync(string unitPriceAssetId, UnitPriceAssetWriteDTO dto);
        Task<IEnumerable<UnitPriceAssetReadDTO>> CreateUnitPriceAssetsAsync(IEnumerable<UnitPriceAssetWriteDTO> dtos);
        Task DeleteUnitPriceAsset(string unitPriceAssetId);

        Task<IEnumerable<UnitPriceAssetReadDTO>> GetUnitPriceAssetsOfProjectAsync(string projectId);
    }
}
