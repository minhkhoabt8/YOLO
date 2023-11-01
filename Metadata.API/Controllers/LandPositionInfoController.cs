using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
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
    [Route("metadata/landPositionInfo")]
    [ApiController]
    public class LandPositionInfoController : ControllerBase
    {
        private readonly ILandPositionInfoService _landPositionInfoService;

        public LandPositionInfoController(ILandPositionInfoService landPositionInfoService)
        {
            _landPositionInfoService = landPositionInfoService;
        }


        /// <summary>
        /// Query Land Position Info
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ApiPaginatedOkResponse<LandPositionInfoReadDTO>>))]
        public async Task<IActionResult> QuerylandPositionInfo([FromQuery] LandPositionInfoQuery query)
        {
            var landPosition = await _landPositionInfoService.LandPositionInfoQueryAsync(query);

            return ResponseFactory.Ok(landPosition);
        }


        /// <summary>
        /// Get Land Position Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandPositionInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetLandPositionInfoAsync(string id)
        {
            var landPosition = await _landPositionInfoService.GetLandPositionInfoAsync(id);
            return ResponseFactory.Ok(landPosition);
        }

        /// <summary>
        /// Create New Land Position Info
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandPositionInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandPositionInfoAsync(LandPositionInfoWriteDTO writeDTO)
        {
            var landPosition = await _landPositionInfoService.CreateLandPositionInfoAsync(writeDTO);

            return ResponseFactory.Created(landPosition);
        }

        /// <summary>
        /// Update Land Position Info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandPositionInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateLandPositionInfoAsync(string id, LandPositionInfoWriteDTO writeDTO)
        {
            var landPosition = await _landPositionInfoService.UpdateLandPositionInfoAsync(id, writeDTO);

            return ResponseFactory.Ok(landPosition);
        }


        /// <summary>
        /// Delete Land Position Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLandPositionInfo(string id)
        {
            await _landPositionInfoService.DeleteLandPositionInfo(id);
            return ResponseFactory.NoContent();
        }
    }
}
