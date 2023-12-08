using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;


namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IUnitPriceLandService
    {
        Task<PaginatedResponse<UnitPriceLandReadDTO>> UnitPriceLandQueryAsync(UnitPriceLandQuery query);
        Task<UnitPriceLandReadDTO> GetUnitPriceLandAsync(string unitPriceLandId);
        Task<UnitPriceLandReadDTO> CreateUnitPriceLandAsync(UnitPriceLandWriteDTO dto);
        Task<UnitPriceLandReadDTO> UpdateUnitPriceLandAsync(string unitPriceLandId, UnitPriceLandWriteDTO dto);
        Task<IEnumerable<UnitPriceLandReadDTO>> CreateUnitPriceLandsAsync(IEnumerable<UnitPriceLandWriteDTO> dtos);
        Task<IEnumerable<UnitPriceLandReadDTO>> GetUnitPriceLandOfProjectAsync(string projectId);
        Task<IEnumerable<UnitPriceLandReadDTO>> ImportUnitPriceLandFromExcelFileAsync(IFormFile file);
        Task DeleteUnitPriceLand(string unitPriceLandId);
    }
}
