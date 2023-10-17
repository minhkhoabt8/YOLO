using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/assetUnit")]
    [ApiController]
    public class AssetUnitController : Controller
    {
        private readonly IAssetUnitService _assetUnitService;

        public AssetUnitController(IAssetUnitService assetUnitService)
        {
            _assetUnitService = assetUnitService;
        }

        /// <summary>
        /// Get all AssetUnits
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetUnitReadDTO>>))]
        public async Task<IActionResult> getAllAssetUnits()
        {
            var assetUnits = await _assetUnitService.GetAllAssetUnitAsync();
            return ResponseFactory.Ok(assetUnits);
        }

        /// <summary>
        /// Get AssetUnits
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        public async Task<IActionResult> getAssetUnit(string id)
        {
            var assetUnit = await _assetUnitService.GetAsync(id);
            return ResponseFactory.Ok(assetUnit);
        }


        /// <summary>
        /// Create new AssetUnits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetUnit(AssetUnitWriteDTO input)
        {
            var assetUnit = await _assetUnitService.CreateAssetUnitAsync(input);
            return ResponseFactory.Created(assetUnit);
        }


        /// <summary>
        /// Update AssetUnit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAssetUnit(string id, AssetUnitWriteDTO writeDTO)
        {
            var assetUnit = await _assetUnitService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(assetUnit);
        }

        /// <summary>
        /// Delete AssetUnit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetUnit(string id)
        {
            await _assetUnitService.DeleteAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
