using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/landType")]
    [ApiController]
    public class LandTypeController : Controller
    {
        private readonly ILandTypeService _landTypeService;

        public LandTypeController(ILandTypeService landTypeService)
        {
            _landTypeService = landTypeService;
        }

        /// <summary>
        /// Get all LandType
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandTypeReadDTO>>))]
        public async Task<IActionResult> GetAllLandType()
        {
            var landTypes = await _landTypeService.GetAllLandTypeAsync();
            return ResponseFactory.Ok(landTypes);

        }

        /// <summary>
        /// Get all LandType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllDeleted")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandTypeReadDTO>>))]
        public async Task<IActionResult> getAllDeletedLandType()
        {
            var landTypes = await _landTypeService.GetAllDeletedLandTypeAsync();
            return ResponseFactory.Ok(landTypes);

        }


        /// <summary>
        /// Get LandType
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        public async Task<IActionResult> getLandType(string id)
        {
            var landType = await _landTypeService.GetAsync(id);
            return ResponseFactory.Ok(landType);
        }

        /// <summary>
        /// Create new LandType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost()]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandType(LandTypeWriteDTO input)
        {
            var landType = await _landTypeService.CreateLandTypeAsync(input);
            return ResponseFactory.Created(landType);
        }

        /// <summary>
        /// Update LandType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateLandType(string id, LandTypeWriteDTO writeDTO)
        {
            var landType = await _landTypeService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(landType);
        }

        /// <summary>
        /// Delete LandType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteLandGroup(string id)
        {
            await _landTypeService.DeleteAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
