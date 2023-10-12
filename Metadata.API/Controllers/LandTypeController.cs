using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/project")]
    [ApiController]
    public class LandTypeController : Controller
    {
        private readonly ILandTypeService _landTypeService;

        public LandTypeController(ILandTypeService landTypeService)
        {
            _landTypeService = landTypeService;
        }
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandTypeReadDTO>>))]
        public async Task<IActionResult> getAllLandType()
        {
            var landTypes = await _landTypeService.GetAllLandTypeAsync();
            return ResponseFactory.Ok(landTypes);

        }
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandType(LandTypeWriteDTO writeDTO)
        {
            var landType = await _landTypeService.CreateLandTypeAsync(writeDTO);
            return ResponseFactory.Created(landType);
        }

        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateLandType(string id, LandTypeWriteDTO writeDTO)
        {
            var landType = await _landTypeService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(landType);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLandGroup(LandTypeWriteDTO ob)
        {
            await _landTypeService.DeleteAsync(ob);
            return ResponseFactory.NoContent();
        }
    }
}
