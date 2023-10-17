using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/deductionType")]
    [ApiController]
    public class DeductionTypeController : Controller
    {
        private readonly IDeductionTypeService _deductionTypeService;

        public DeductionTypeController(IDeductionTypeService deductionTypeService)
        {
            _deductionTypeService = deductionTypeService;
        }

        /// <summary>
        /// Get all DeductionTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DeductionTypeReadDTO>>))]
        public async Task<IActionResult> getAllDeductionTypes()
        {
            var deductionTypes = await _deductionTypeService.GetAllDeductionTypesAsync();
            return ResponseFactory.Ok(deductionTypes);
        }

        /// <summary>
        /// Get DeductionTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        public async Task<IActionResult> getDeductionType(string id)
        {
            var deductionType = await _deductionTypeService.GetDeductionTypeAsync(id);
            return ResponseFactory.Ok(deductionType);
        }

        /// <summary>
        /// Create new DeductionTypes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateDeductionType(DeductionTypeWriteDTO input)
        {
            var deductionType = await _deductionTypeService.AddDeductionType(input);
            return ResponseFactory.Created(deductionType);
        }

        /// <summary>
        /// Update DeductionTypes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateDeductionType(string id, DeductionTypeWriteDTO writeDTO)
        {
            var deductionType = await _deductionTypeService.UpdateDeductionTypeAsync(id, writeDTO);
            return ResponseFactory.Ok(deductionType);
        }

        /// <summary>
        /// Delete DeductionTypes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteDeductionType(string id)
        {
            await _deductionTypeService.DeleteDeductionTypeAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
