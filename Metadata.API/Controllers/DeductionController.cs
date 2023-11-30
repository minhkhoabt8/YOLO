using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/Deduction")]
    [ApiController]
    public class DeductionController : Controller
    {
       private readonly IDeductionService _deductionService;

        public DeductionController(IDeductionService deductionService)
        {
            _deductionService = deductionService;
        }

        /// <summary>
        /// Get All Deductions
        /// </summary>
        /// <param name="ownerId</param>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DeductionReadDTO>>))]
        public async Task<IActionResult> GetAllDeductions(string ownerId)
        {
            var deductions = await _deductionService.GetDeductionsAsync(ownerId);
            return ResponseFactory.Ok(deductions);
        }


        /// <summary>
        /// Create list deductions
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiCreatedResponse<DeductionReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateListDeductions(string id, IEnumerable<DeductionWriteDTO> input)
        {
            var deductions = await _deductionService.CreateOwnerDeductionsAsync(id, input);
            return ResponseFactory.Created(deductions);
        }

        /// <summary>
        /// Update deduction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateDeduction(string id, DeductionWriteDTO writeDTO)
        {
            var deductions = await _deductionService.UpdateDeductionAsync(id, writeDTO);
            return ResponseFactory.Ok(deductions);
        }

        /// <summary>
        /// Delete deduction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteSupport(string id)
        {
            await _deductionService.DeleteDeductionAsync(id);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Query  deduction
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DeductionReadDTO>>))]
        public async Task<IActionResult> QuerySupport([FromQuery] DeductionQuery query)
        {
            var deductions = await _deductionService.QueryDeductionAsync(query);
            return ResponseFactory.PaginatedOk(deductions);
        }
    }
}
