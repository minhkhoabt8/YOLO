using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IGCNLandInfoService
    {
        Task<GCNLandInfoReadDTO> CreateGCNLandInfoAsync(GCNLandInfoWriteDTO dto);
        Task<IEnumerable<GCNLandInfoReadDTO>> GetAllGCNLandInfosAsync();
        Task<GCNLandInfoReadDTO> UpdateGCNLandInfoAsync(string id, GCNLandInfoWriteDTO dto);
        Task DeleteGCNLandInfoAsync(string id);
        Task<GCNLandInfoReadDTO> GetGCNLandInfoAsync(string id);
        Task<PaginatedResponse<GCNLandInfoReadDTO>> GCNLandInfoQueryAsync(GCNLandInfoQuery query);
    }
}
