using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{

        [Route("metadata/project")]
        [ApiController]
    public class LandGroupController : ControllerBase
    {
        private readonly ILandGroupService _landGroupService;

        public LandGroupController(ILandGroupService landGroupService)
        {
            _landGroupService = landGroupService;
        }

        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandGroupReadDTO>>))]
        public async Task<IActionResult> getAllLandGroups()
        {
            var landGroups = await _landGroupService.GetAllLandGroupAsync();
            return ResponseFactory.Ok(landGroups);
        }


        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandGroup(LandGroupWriteDTO writeDTO)
        {
            var landGroup = await _landGroupService.CreateLandgroupAsync(writeDTO);
            return ResponseFactory.Created(landGroup);
        }

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

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandGroupReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLandGroup(LandGroupWriteDTO ob)
        {
            await _landGroupService.DeleteAsync(ob);
            return ResponseFactory.NoContent();
        }
    }
}
