using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetUnitController : Controller
    {
        private readonly IAssetUnitService _assetUnitService;

        public AssetUnitController(IAssetUnitService assetUnitService)
        {
            _assetUnitService = assetUnitService;
        }

        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetUnitReadDTO>>))]
        public async Task<IActionResult> getAllAssetUnits()
        {
            var assetUnits = await _assetUnitService.GetAllAssetUnitAsync();
            return ResponseFactory.Ok(assetUnits);
        }

        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetUnit(AssetUnitWriteDTO writeDTO)
        {
            var assetUnit = await _assetUnitService.CreateAssetUnitAsync(writeDTO);
            return ResponseFactory.Created(assetUnit);
        }

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

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetUnit(string ob)
        {
            await _assetUnitService.DeleteAsync(ob);
            return ResponseFactory.NoContent();
        }
    }
}
