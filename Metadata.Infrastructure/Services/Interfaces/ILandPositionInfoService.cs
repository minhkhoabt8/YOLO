using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ILandPositionInfoService
    {
        Task<PaginatedResponse<LandPositionInfoReadDTO>> LandPositionInfoQueryAsync(LandPositionInfoQuery query);
        Task<LandPositionInfoReadDTO> GetLandPositionInfoAsync(string id);
        Task<LandPositionInfoReadDTO> CreateLandPositionInfoAsync(LandPositionInfoWriteDTO dto);
        Task<LandPositionInfoReadDTO> UpdateLandPositionInfoAsync(string idd, LandPositionInfoWriteDTO dto);
        Task DeleteLandPositionInfo(string unitPriceLandId);
    }
}
