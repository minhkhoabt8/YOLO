using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;
using System.ComponentModel.DataAnnotations;

namespace Metadata.API.Controllers
{
    [Route("metadata/gcnLandInfo")]
    [ApiController]
    public class GCNLandInfoController : ControllerBase
    {
        private readonly IGCNLandInfoService _gcnLandInfoService;

        public GCNLandInfoController(IGCNLandInfoService gcnLandInfoRepository)
        {
            _gcnLandInfoService = gcnLandInfoRepository;
        }

        /// <summary>
        /// Query GCN Land Info
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<GCNLandInfoReadDTO>))]
        public async Task<IActionResult> QueryGCNLandInfos([FromQuery] GCNLandInfoQuery query)
        {
            var gcnLandInfos = await _gcnLandInfoService.GCNLandInfoQueryAsync(query);

            return ResponseFactory.PaginatedOk(gcnLandInfos);
        }

        /// <summary>
        /// Get GCN Land Info Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GCNLandInfoReadDTO>))]
        public async Task<IActionResult> GetGCNLandInfosDetails(string id)
        {
            var gcnLandInfo = await _gcnLandInfoService.GetGCNLandInfoAsync(id);

            return ResponseFactory.Ok(gcnLandInfo);
        }

        /// <summary>
        /// Create GCN Land Info
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<GCNLandInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateGCNLandInfoAsync(GCNLandInfoWriteDTO dto)
        {
            var gcnLandInfo = await _gcnLandInfoService.CreateGCNLandInfoAsync(dto);

            return ResponseFactory.Created(gcnLandInfo);
        }

        /// <summary>
        /// Create GCN Land Info From List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("creates")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<GCNLandInfoReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateGCNLandInfosAsync(IEnumerable<GCNLandInfoWriteDTO> dtos)
        {
            var gcnLandInfo = await _gcnLandInfoService.CreateGCNLandInfosAsync(dtos);

            return ResponseFactory.Created(gcnLandInfo);
        }

        /// <summary>
        /// Check Duplicate GCN Land Info
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="plotNumber"></param>
        /// <returns></returns>
        [HttpPost("duplicate")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<GCNLandInfoReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CheckDuplicateGCNLandInfoAsync([Required] string pageNumber, [Required] string plotNumber )
        {
            var gcnLandInfo = await _gcnLandInfoService.CheckDuplicateGCNLandInfoAsync(pageNumber, plotNumber);

            return ResponseFactory.Ok(gcnLandInfo);
        }


        /// <summary>
        /// Update GCN Land Info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<GCNLandInfoReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateGCNLandInfo(string id, GCNLandInfoWriteDTO writeDTO)
        {
            var gcnLandInfo = await _gcnLandInfoService.UpdateGCNLandInfoAsync(id, writeDTO);
            return ResponseFactory.Ok(gcnLandInfo);
        }

        /// <summary>
        /// Delete GCN Land Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteGCNLandInfo(string id)
        {
            await _gcnLandInfoService.DeleteGCNLandInfoAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
