using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/project")]
    [ApiController]
    public class AssetGroupController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;

        public AssetGroupController(IAssetGroupService assetGroupService)
        {
            _assetGroupService = assetGroupService;
        }

        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> getAllAssetGroups()
        {
            var assetGroups = await _assetGroupService.GetAllAssetGroupsAsync();
            return ResponseFactory.Ok(assetGroups);
        }

        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetGroup(AssetGroupWriteDTO writeDTO)
        {
            var assetGroup = await _assetGroupService.CreateAssetGroupAsync(writeDTO);
            return ResponseFactory.Created(assetGroup);
        }

        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAssetGroup(string id, AssetGroupWriteDTO writeDTO)
        {
            var assetGroup = await _assetGroupService.UpdateAssetGroupAsync(id, writeDTO);
            return ResponseFactory.Ok(assetGroup);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetGroup(string ob)
        {
            await _assetGroupService.DeleteAssetGroupAsync(ob);
            return ResponseFactory.NoContent();
        }
    }
}
