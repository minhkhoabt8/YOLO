using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{

        [Route("metadata/landGroup")]
        [ApiController]
    public class LandGroupController : ControllerBase
    {
        private readonly ILandGroupService _landGroupService;

        public LandGroupController(ILandGroupService landGroupService)
        {
            _landGroupService = landGroupService;
        }

        /// <summary>
        /// Get all LandGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandGroupReadDTO>>))]
        public async Task<IActionResult> getAllLandGroups()
        {
            var landGroups = await _landGroupService.GetAllLandGroupAsync();
            return ResponseFactory.Ok(landGroups);
        }

        /// <summary>
        /// Get LandGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        public async Task<IActionResult> getLandGroup(string id)
        {
            var landGroup = await _landGroupService.GetAsync(id);
            return ResponseFactory.Ok(landGroup);
        }

        /// <summary>
        /// Create new LandGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandGroup(LandGroupWriteDTO input)
        {
            var landGroup = await _landGroupService.CreateLandgroupAsync(input);
            return ResponseFactory.Created(landGroup);
        }

        /// <summary>
        /// Update LandGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateLandGroup(string id, LandGroupWriteDTO writeDTO)
        {
            var landGroup = await _landGroupService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(landGroup);
        }

        /// <summary>
        /// Delete LandGroup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLandGroup(string id)
        {
            await _landGroupService.DeleteAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
