using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
