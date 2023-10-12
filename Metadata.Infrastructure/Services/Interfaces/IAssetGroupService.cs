using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAssetGroupService
    {
        Task<IEnumerable<AssetGroupReadDTO>> GetAllAssetGroupsAsync();
        Task<AssetGroupReadDTO> GetAssetGroupAsync(string code);
        Task<AssetGroupReadDTO> CreateAssetGroupAsync(AssetGroupWriteDTO assetGroupWriteDTO);
        Task<AssetGroupReadDTO> UpdateAssetGroupAsync(string id ,AssetGroupWriteDTO assetGroupWriteDTO);
        Task<bool> DeleteAssetGroupAsync(string id);
    }
}
