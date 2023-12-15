using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;
using System.ComponentModel.DataAnnotations;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("metadata/unitPriceLand")]
    [ApiController]
    public class UnitPriceLandController : ControllerBase
    {
        private readonly IUnitPriceLandService _unitPriceLandService;

        public UnitPriceLandController(IUnitPriceLandService unitPriceLandService)
        {
            _unitPriceLandService = unitPriceLandService;
        }

        /// <summary>
        /// Query Unit Price Land
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ApiPaginatedOkResponse<UnitPriceLandReadDTO>>))]
        public async Task<IActionResult> QueryUnitPriceLand([FromQuery] UnitPriceLandQuery query)
        {
            var unitPriceLand = await _unitPriceLandService.UnitPriceLandQueryAsync(query);

            return ResponseFactory.PaginatedOk(unitPriceLand);
        }

        /// <summary>
        /// Get Unit Price Land Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UnitPriceLandReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUnitPriceLandDetailsAsync(string id)
        {
            var unitPriceLand = await _unitPriceLandService.GetUnitPriceLandAsync(id);

            return ResponseFactory.Ok(unitPriceLand);
        }

        /// <summary>
        /// Get Unit Price Land Of Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("project")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ApiPaginatedOkResponse<UnitPriceLandReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUnitPriceLandsOfProjectAsync([Required][FromQuery]string projectId, [FromQuery] UnitPriceLandQuery query )
        {
            var unitPriceLands = await _unitPriceLandService.QueryUnitPriceLandOfProjectAsync(projectId, query);

            return ResponseFactory.Ok(unitPriceLands);
        }

        /// <summary>
        /// Create New Unit Price Land
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<UnitPriceLandReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateUnitPriceLandAsync(UnitPriceLandWriteDTO writeDTO)
        {
            var unitPriceLand = await _unitPriceLandService.CreateUnitPriceLandAsync(writeDTO);

            return ResponseFactory.Created(unitPriceLand);
        }

        /// <summary>
        /// Create Unit Price Land From List
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("creates")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable< UnitPriceLandReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateUnitPriceLandsAsync(IEnumerable<UnitPriceLandWriteDTO> writeDTO)
        {
            var unitPriceLands = await _unitPriceLandService.CreateUnitPriceLandsAsync(writeDTO);

            return ResponseFactory.Created(unitPriceLands);
        }

        /// <summary>
        /// Import Unit Price Land From File
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> ImportUnitPriceLandFromExcelFileAsync([Required] IFormFile file)
        {
            var result = await _unitPriceLandService.ImportUnitPriceLandFromExcelFileAsync(file);
            return ResponseFactory.Created(result);
        }

        /// <summary>
        /// Update Unit Price Land
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UnitPriceLandReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUnitPriceLandAsync(string id, UnitPriceLandWriteDTO writeDTO)
        {
            var unitPriceLand = await _unitPriceLandService.UpdateUnitPriceLandAsync(id, writeDTO);
            return ResponseFactory.Ok(unitPriceLand);
        }


        /// <summary>
        /// Delete Unit Price Land
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteUnitPriceLand(string id)
        {
            await _unitPriceLandService.DeleteUnitPriceLand(id);
            return ResponseFactory.NoContent();
        }
    }
}
