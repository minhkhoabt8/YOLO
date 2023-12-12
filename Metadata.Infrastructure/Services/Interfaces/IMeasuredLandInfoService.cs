using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Project;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IMeasuredLandInfoService
    {
        Task<MeasuredLandInfoReadDTO> CreateMeasuredLandInfoAsync(MeasuredLandInfoWriteDTO dto);
        Task<IEnumerable<MeasuredLandInfoReadDTO>> GetAllMeasuredLandInfosAsync();
        Task<MeasuredLandInfoReadDTO> UpdateMeasuredLandInfoAsync(string id, MeasuredLandInfoWriteDTO dto);
        Task DeleteMeasuredLandInfoAsync(string id);
        Task<MeasuredLandInfoReadDTO> GetMeasuredLandInfoAsync(string id);
        Task CreateMeasuredLandInfoFromFileAsync(IFormFile formFile);
        Task<PaginatedResponse<MeasuredLandInfoReadDTO>> MeasuredLandInfoQueryAsync(MeasuredLandInfoQuery query);

        Task<IEnumerable<MeasuredLandInfoReadDTO>> CreateOwnerMeasuredLandInfosAsync(string ownerId, IEnumerable<MeasuredLandInfoWriteDTO> dto);
        Task<MeasuredLandInfoReadDTO?> CheckDuplicateMeasuredLandInfoAsync(string pageNumber, string plotNumber);
    }
}
