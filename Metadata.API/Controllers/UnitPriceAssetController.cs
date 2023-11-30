using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("metadata/unitPriceAsset")]
    [ApiController]
    public class UnitPriceAssetController : ControllerBase
    {
        private readonly IUnitPriceAssetService _unitPriceAssetService;

        public UnitPriceAssetController(IUnitPriceAssetService unitPriceAssetService)
        {
            _unitPriceAssetService = unitPriceAssetService;
        }

        /// <summary>
        /// Query Unit Price Asset
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ApiPaginatedOkResponse<UnitPriceAssetReadDTO>>))]
        public async Task<IActionResult> QueryUnitPriceAsset([FromQuery] UnitPriceAssetQuery query)
        {
            var unitPriceAsset = await _unitPriceAssetService.UnitPriceAssetQueryAsync(query);

            return ResponseFactory.PaginatedOk(unitPriceAsset);
        }


        /// <summary>
        /// Get Unit Price Asset Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UnitPriceAssetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUnitPriceAssetDetailsAsync(string id)
        {
            var unitPriceAsset = await _unitPriceAssetService.GetUnitPriceAssetAsync(id);
            return ResponseFactory.Ok(unitPriceAsset);
        }

        /// <summary>
        /// Get Unit Price Assets Of Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("project")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<UnitPriceAssetReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetUnitPriceAssetsOfProjectAsync(string projectId)
        {
            var unitPriceAssets = await _unitPriceAssetService.GetUnitPriceAssetsOfProjectAsync(projectId);
            return ResponseFactory.Ok(unitPriceAssets);
        }


        /// <summary>
        /// Create New Unit Price Asset
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<UnitPriceAssetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateUnitPriceAssetAsync(UnitPriceAssetWriteDTO writeDTO)
        {
            var unitPriceAsset = await _unitPriceAssetService.CreateUnitPriceAssetAsync(writeDTO);

            return ResponseFactory.Created(unitPriceAsset);
        }

        /// <summary>
        /// Create New Unit Price Asset From List
        /// </summary>
        /// <param name="writeDTOs"></param>
        /// <returns></returns>
        [HttpPost("creates")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<UnitPriceAssetReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateUnitPriceAssetsAsync(IEnumerable<UnitPriceAssetWriteDTO> writeDTOs)
        {
            var unitPriceAssets = await _unitPriceAssetService.CreateUnitPriceAssetsAsync(writeDTOs);

            return ResponseFactory.Created(unitPriceAssets);
        }



        /// <summary>
        /// Update Unit Price Asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<UnitPriceAssetReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateUnitPriceAssetAsync(string id, UnitPriceAssetWriteDTO writeDTO)
        {
            var unitPriceAsset = await _unitPriceAssetService.UpdateUnitPriceAssetAsync(id, writeDTO);
            return ResponseFactory.Ok(unitPriceAsset);
        }


        /// <summary>
        /// Delete Unit Price Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteUnitPriceAsset(string id)
        {
            await _unitPriceAssetService.DeleteUnitPriceAsset(id);
            return ResponseFactory.NoContent();
        }

    }
}
