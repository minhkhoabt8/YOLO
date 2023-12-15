using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/Support")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        /// <summary>
        /// Get All Supports
        /// </summary>
        /// <param name="ownerId</param>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportReadDTO>>))]
        public async Task<IActionResult> GetAllSupports(string ownerId)
        {
            var supports = await _supportService.GetSupportsAsync(ownerId);
            return ResponseFactory.Ok(supports);
        }


        /// <summary>
        /// Create list support
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<SupportReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateListSupport(string id, IEnumerable<SupportWriteDTO> input)
        {
            var supports = await _supportService.CreateOwnerSupportsAsync(id, input);
            return ResponseFactory.Created(supports);
        }

        /// <summary>
        /// Update support
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateSupport(string id, SupportWriteDTO writeDTO)
        {
            var support = await _supportService.UpdateSupportAsync(id, writeDTO);
            return ResponseFactory.Ok(support);
        }

        /// <summary>
        /// Delete support
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteSupport(string id)
        {
            await _supportService.DeleteSupportAsync(id);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Query  support
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportReadDTO>>))]
        public async Task<IActionResult> QuerySupport([FromQuery] SupportQuery query)
        {
            var supports = await _supportService.QuerySupportAsync(query);
            return ResponseFactory.PaginatedOk(supports);
        }
    }
}
