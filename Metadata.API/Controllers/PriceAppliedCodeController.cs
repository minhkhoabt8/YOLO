using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("metadata/priceAplliedCode")]
    [ApiController]
    public class PriceAppliedCodeController : ControllerBase
    {
        private readonly IPriceAppliedCodeService _priceAppliedCodeService;

        public PriceAppliedCodeController(IPriceAppliedCodeService priceAppliedCodeService)
        {
            _priceAppliedCodeService = priceAppliedCodeService;

        }
        /// <summary>
        /// Query Price Applied Code
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<PriceAppliedCodeReadDTO>>))]
        public async Task<IActionResult> QueryPriceAplliedCode([FromQuery] PriceAppliedCodeQuery query)
        {
            var priceAppliedCode = await _priceAppliedCodeService.QueryPriceAppliedCodeAsync(query);

            return ResponseFactory.PaginatedOk(priceAppliedCode);
        }

        /// <summary>
        /// Get Price Applied Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<PriceAppliedCodeReadDTO>>))]
        public async Task<IActionResult> GetAllPriceAplliedCode(string Id)
        {
            var priceAppliedCode = await _priceAppliedCodeService.GetPriceAppliedCodeAsync(Id);
            return ResponseFactory.Ok(priceAppliedCode);
        }

        /// <summary>
        /// Check duplicate code
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateCode")]
        public async Task<IActionResult> CheckDuplicateCode(string code)
        {
            await _priceAppliedCodeService.CheckDuplicateCodeAsync(code);
            return ResponseFactory.Accepted();
        }


        /// <summary>
        /// Create New Price Applied Code
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<PriceAppliedCodeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreatePriceAplliedCodeAsync(PriceAppliedCodeWriteDTO dto)
        {
            var priceAppliedCode = await _priceAppliedCodeService.CreatePriceAppliedCodeAsync(dto);
            return ResponseFactory.Created(priceAppliedCode);
        }

        /// <summary>
        /// Create New Price Applied Code From List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("creates")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<PriceAppliedCodeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreatePriceAplliedCodesAsync(IEnumerable<PriceAppliedCodeWriteDTO> dto)
        {
            var priceAppliedCode = await _priceAppliedCodeService.CreatePriceAppliedCodesAsync(dto);
            return ResponseFactory.Created(priceAppliedCode);
        }

        /// <summary>
        /// Update Price Applied Code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PriceAppliedCodeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePriceAplliedCode(string id, PriceAppliedCodeWriteDTO writeDTO)
        {
            var priceAppliedCode = await _priceAppliedCodeService.UpdatePriceAppliedCodeAsync(id, writeDTO);
            return ResponseFactory.Ok(priceAppliedCode);
        }

        /// <summary>
        /// Delete Price Applied Code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeletePriceAplliedCode(string id)
        {
            await _priceAppliedCodeService.DeletePriceAppliedCodeAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
