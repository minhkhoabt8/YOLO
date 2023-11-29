using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Services.Implementations;
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
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DeductionTypeReadDTO>>))]
        public async Task<IActionResult> GetAllDeductionTypes()
        {
            var deductionTypes = await _deductionTypeService.GetAllDeductionTypesAsync();
            return ResponseFactory.Ok(deductionTypes);
        }

        /// <summary>
        /// GetActivedDeductionTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("getActived")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DeductionTypeReadDTO>>))]
        public async Task<IActionResult> GetActivedDeductionTypes()
        {
            var deductionTypes = await _deductionTypeService.GetActivedDeductionTypes();
            return ResponseFactory.Ok(deductionTypes);
        }

        /// <summary>
        /// Get DeductionTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        public async Task<IActionResult> GetDeductionType(string id)
        {
            var deductionType = await _deductionTypeService.GetDeductionTypeAsync(id);
            return ResponseFactory.Ok(deductionType);
        }

        /// <summary>
        /// Create new DeductionTypes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateDeductionType(DeductionTypeWriteDTO input)
        {
            var deductionType = await _deductionTypeService.AddDeductionType(input);
            return ResponseFactory.Created(deductionType);
        }

        /// <summary>
        /// Create list deductionTypes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<DeductionTypeReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateListDeductionTypes(IEnumerable<DeductionTypeWriteDTO> input)
        {
            var deductionTypes = await _deductionTypeService.CreateListDeductionTypes(input);
            return ResponseFactory.Created(deductionTypes);
        }

        /// <summary>
        /// Update DeductionTypes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
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
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DeductionTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteDeductionType(string id)
        {
            await _deductionTypeService.DeleteDeductionTypeAsync(id);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Check duplicate code
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateCode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateCode(string code)
        {
            await _deductionTypeService.CheckcodeDeductionTypeNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Check duplicate Name
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateName(string name)
        {
            await _deductionTypeService.ChecknameDeductionTypeNotDuplicate(name);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  DeductionTypes
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DeductionTypeReadDTO>>))]
        public async Task<IActionResult> QueryDeductionTypes([FromQuery] DeductionTypeQuery query)
        {
            var deductionTypes = await _deductionTypeService.QueryDeductionTypesAsync(query);
            return ResponseFactory.Ok(deductionTypes);
        }

        //import data from excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportDeductionType(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string filePath = Path.GetTempFileName();

            // Save the uploaded file to a temporary file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                var dataImport = await _deductionTypeService.ImportDeductionTypeFromExcelAsync(filePath);
                return Ok(new { Message = "Deduction types imported successfully", Data = dataImport });
               
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
