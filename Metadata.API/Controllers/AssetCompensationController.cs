using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/AssetCompensation")]
    [ApiController]
    public class AssetCompensationController : ControllerBase
    {
        private readonly IAssetCompensationService _assetCompensationService;

        public AssetCompensationController(IAssetCompensationService assetCompensationService)
        {
            _assetCompensationService = assetCompensationService;
        }

        /// <summary>
        /// Get AllAssetCompensations
        /// </summary>
         /// <param name="ownerId</param>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetCompensationReadDTO>>))]
        public async Task<IActionResult> GetAllAssetCompensations(string ownerId)
        {
            var assetCompensations = await _assetCompensationService.GetAssetCompensationsAsync(ownerId);
            return ResponseFactory.Ok(assetCompensations);
        }

        /// <summary>
        /// Create list assetCompensation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<AssetCompensationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateListAssetCompensation( string id , IEnumerable<AssetCompensationWriteDTO> input)
        {
            var assetCompensations = await _assetCompensationService.CreateOwnerAssetCompensationsAsync(id , input);
            return ResponseFactory.Created(assetCompensations);
        }


        /// <summary>
        /// Update AssetCompensation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetCompensationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAssetCompensation(string id, AssetCompensationWriteDTO writeDTO)
        {
            var assetCompensation = await _assetCompensationService.UpdateAssetCompensationAsync(id, writeDTO);
            return ResponseFactory.Ok(assetCompensation);
        }


        /// <summary>
        /// Delete AssetCompensation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetCompensationReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAssetCompensation(string id)
        {
            await _assetCompensationService.DeleteAssetCompensationAsync(id);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Query  Asset Group
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AssetCompensationReadDTO>>))]
        public async Task<IActionResult> QueryAssetCompensation([FromQuery] AssetCompensationQuery query)
        {
            var assetCompensations = await _assetCompensationService.QueryAssetCompensationAsync(query);
            return ResponseFactory.PaginatedOk(assetCompensations);
        }
    }
}
