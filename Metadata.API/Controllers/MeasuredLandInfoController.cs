using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;
using System.ComponentModel.DataAnnotations;

namespace Metadata.API.Controllers
{
    [Route("metadata/measuredLandInfo")]
    [ApiController]
    public class MeasuredLandInfoController : ControllerBase
    {
        private readonly IMeasuredLandInfoService _measuredLandInfoService;

        public MeasuredLandInfoController(IMeasuredLandInfoService measuredLandInfoService)
        {
            _measuredLandInfoService = measuredLandInfoService;
        }

        /// <summary>
        /// Query Measured Land Info
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<MeasuredLandInfoReadDTO>))]
        public async Task<IActionResult> QueryMeasuredLandInfos([FromQuery] MeasuredLandInfoQuery query)
        {
            var measuredLandInfos = await _measuredLandInfoService.MeasuredLandInfoQueryAsync(query);

            return ResponseFactory.PaginatedOk(measuredLandInfos);
        }


        /// <summary>
        /// Get Measured Land Info Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MeasuredLandInfoReadDTO>))]
        public async Task<IActionResult> GetMeasuredLandInfosDetails(string id)
        {
            var measuredLandInfo = await _measuredLandInfoService.GetMeasuredLandInfoAsync(id);

            return ResponseFactory.Ok(measuredLandInfo);
        }

        /// <summary>
        /// Create Measured Land Info
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<MeasuredLandInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateMeasuredLandInfoAsync([FromForm] MeasuredLandInfoWriteDTO dto)
        {
            var measuredLandInfo = await _measuredLandInfoService.CreateMeasuredLandInfoAsync(dto);

            return ResponseFactory.Created(measuredLandInfo);
        }

        /// <summary>
        /// Check Duplicate Measured Land Info
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="plotNumber"></param>
        /// <returns></returns>
        [HttpPost("duplicate")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<GCNLandInfoReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CheckDuplicateMeasuredLandInfoAsync([Required] string pageNumber, [Required] string plotNumber)
        {
            var measuredLandInfo = await _measuredLandInfoService.CheckDuplicateMeasuredLandInfoAsync(pageNumber, plotNumber);

            return ResponseFactory.Ok(measuredLandInfo);
        }

        /// <summary>
        /// Update Measured Land Info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<MeasuredLandInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateMeasuredLandInfo(string id, MeasuredLandInfoWriteDTO writeDTO)
        {
            var measuredLandInfo = await _measuredLandInfoService.UpdateMeasuredLandInfoAsync(id, writeDTO);
            return ResponseFactory.Ok(measuredLandInfo);
        }

        /// <summary>
        /// Delete Measured Land Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteMeasuredLandInfo(string id)
        {
            await _measuredLandInfoService.DeleteMeasuredLandInfoAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
