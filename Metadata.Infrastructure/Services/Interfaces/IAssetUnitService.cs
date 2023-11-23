using Metadata.Infrastructure.DTOs.AssetUnit;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAssetUnitService
    {
        Task<IEnumerable<AssetUnitReadDTO>> GetAllAssetUnitAsync();
        Task<IEnumerable<AssetUnitReadDTO>> GetActivedAssetUnitAsync();
        Task<AssetUnitReadDTO?> GetAsync(string code);
        Task<AssetUnitReadDTO?> CreateAssetUnitAsync(AssetUnitWriteDTO assetUnitWriteDTO);
        Task<AssetUnitReadDTO?> UpdateAsync(string id, AssetUnitWriteDTO assetUnitUpdateDTO);
        Task<bool> DeleteAsync(string delete);
        Task CheckNameAssetUnitNotDuplicate(string name);
        Task CheckCodeAssetUnitNotDuplicate(string code);
        Task<IEnumerable<AssetUnitReadDTO>> CreateListAssetUnitAsync(IEnumerable<AssetUnitWriteDTO> assetUnitWriteDTOs);
        Task<PaginatedResponse<AssetUnitReadDTO>> QueryAssetUnitAsync(AssetUnitQuery paginationQuery);
        Task ImportAssetUnitFromExcelAsync(string filePath);
    }
}
