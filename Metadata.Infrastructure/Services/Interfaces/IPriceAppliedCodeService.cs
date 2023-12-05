using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using SharedLib.Infrastructure.DTOs;


namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IPriceAppliedCodeService
    {
        Task<IEnumerable<PriceAppliedCodeReadDTO>> CreatePriceAppliedCodesAsync(IEnumerable<PriceAppliedCodeWriteDTO> dto);
        Task<PriceAppliedCodeReadDTO> UpdatePriceAppliedCodeAsync(string Id, PriceAppliedCodeWriteDTO dto);
        Task DeletePriceAppliedCodeAsync(string Id);
        Task<PriceAppliedCodeReadDTO> GetPriceAppliedCodeAsync(string Id);
        Task<PaginatedResponse<PriceAppliedCodeReadDTO>> QueryPriceAppliedCodeAsync(PriceAppliedCodeQuery paginationQuery);

        Task<PriceAppliedCodeReadDTO> CheckDuplicateCodeAsync(string code);
        Task<PriceAppliedCodeReadDTO> CreatePriceAppliedCodeAsync(PriceAppliedCodeWriteDTO dto);
        Task<PriceAppliedCodeReadDTO> CreatePriceAplliedDocumentsAsync(string priceAppliedCodeId, IEnumerable<DocumentWriteDTO> documentDtos);
    }
}
