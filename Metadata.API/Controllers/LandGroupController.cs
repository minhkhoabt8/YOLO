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
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandGroupReadDTO>>))]
        public async Task<IActionResult> GetAllLandGroups()
        {
            var landGroups = await _landGroupService.GetAllLandGroupAsync();
            return ResponseFactory.Ok(landGroups);
        }

        /// <summary>
        /// Get all Actived LandGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllActived")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandGroupReadDTO>>))]
        public async Task<IActionResult> getAllActivedLandGroups()
        {
            var landGroups = await _landGroupService.GetAllActivedLandGroupAsync();
            return ResponseFactory.Ok(landGroups);
        }

        /// <summary>
        /// Get LandGroup
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        public async Task<IActionResult> GetLandGroup(string id)
        {
            var landGroup = await _landGroupService.GetAsync(id);
            return ResponseFactory.Ok(landGroup);
        }

        /// <summary>
        /// Create new LandGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandGroup(LandGroupWriteDTO input)
        {
            var landGroup = await _landGroupService.CreateLandgroupAsync(input);
            return ResponseFactory.Created(landGroup);
        }

        /// <summary>
        /// Create new list LandGroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<LandGroupReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateListLandGroup(IEnumerable<LandGroupWriteDTO> input)
        {
            var landGroups = await _landGroupService.CreateListLandGroupAsync(input);
            return ResponseFactory.Created(landGroups);
        }

        /// <summary>
        /// Update LandGroup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
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
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLandGroup(string id)
        {
            await _landGroupService.DeleteAsync(id);
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
            await _landGroupService.CheckNameLandGroupNotDuplicate(name);
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
            await _landGroupService.CheckCodeLandGroupNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  LandGroup
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandGroupReadDTO>>))]
        public async Task<IActionResult> QueryLandGroup([FromQuery] LandGroupQuery query)
        {
            var landGroups = await _landGroupService.QueryLandGroupAsync(query);
            return ResponseFactory.Ok(landGroups);
        }
    }
}
