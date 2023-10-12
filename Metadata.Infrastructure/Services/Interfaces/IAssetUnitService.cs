using Metadata.Infrastructure.DTOs.AssetUnit;
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
        Task<AssetUnitReadDTO?> GetAsync(string code);
        Task<AssetUnitReadDTO?> CreateAssetUnitAsync(AssetUnitWriteDTO assetUnitWriteDTO);
        Task<AssetUnitReadDTO?> UpdateAsync(string id, AssetUnitWriteDTO assetUnitUpdateDTO);
        Task<bool> DeleteAsync(string delete);
    }
}
