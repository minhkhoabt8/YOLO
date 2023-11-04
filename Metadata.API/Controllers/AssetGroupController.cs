using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/assetGroup")]
    [ApiController]
    public class AssetGroupController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;

        public AssetGroupController(IAssetGroupService assetGroupService)
        {
            _assetGroupService = assetGroupService;
        }

        /// <summary>
        /// Get all AssetGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> getAllAssetGroups()
        {
            var assetGroups = await _assetGroupService.GetAllAssetGroupsAsync();
            return ResponseFactory.Ok(assetGroups);
        }

        /// <summary>
        /// Get all Actived AssetGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllActived")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> getAllDeletedAssetGroups()
        {
            var assetGroups = await _assetGroupService.GetAllActivedAssetGroupAsync();
            return ResponseFactory.Ok(assetGroups);
        }

        /// <summary>
        /// get AssetGroup by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        public async Task<IActionResult> getAssetGroup(string id)
        {
            var assetGroup = await _assetGroupService.GetAssetGroupAsync(id);
            return ResponseFactory.Ok(assetGroup);
        }

        /// <summary>
        /// Create new AssetGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetGroup(AssetGroupWriteDTO input)
        {
            var assetGroup = await _assetGroupService.CreateAssetGroupAsync(input);
            return ResponseFactory.Created(assetGroup);
        }

        /// <summary>
        /// Create List AssetGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAssetGroups(IEnumerable<AssetGroupWriteDTO> input)
        {
            var assetGroups = await _assetGroupService.CreateAssetGroupsAsync(input);
            return ResponseFactory.Created(assetGroups);
        }

        /// <summary>
        /// Update AssetGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAssetGroup(string id, AssetGroupWriteDTO writeDTO)
        {
            var assetGroup = await _assetGroupService.UpdateAssetGroupAsync(id, writeDTO);
            return ResponseFactory.Ok(assetGroup);
        }

        /// <summary>
        /// Delete AssetGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetGroup(string id)
        {
            await _assetGroupService.DeleteAssetGroupAsync(id);
            return ResponseFactory.NoContent();
        }


        /// <summary>
        /// Check Duplicate Name
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateName(string name)
        {
             await _assetGroupService.CheckNameAssetGroupNotDuplicate(name);
            return ResponseFactory.Accepted();
        }


        /// <summary>
        /// Check Duplicate Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateCode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateCode(string code)
        {
            await _assetGroupService.CheckCodeAssetGroupNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  Asset Group
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetGroupReadDTO>>))]
        public async Task<IActionResult> QueryAssetGroup([FromQuery] AssetGroupQuery query)
        {
            var assetGroups = await _assetGroupService.QueryAssetGroupAsync(query);
            return ResponseFactory.Ok(assetGroups);
        }
    }
}
