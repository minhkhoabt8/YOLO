using Metadata.Infrastructure.DTOs.SupportType;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/supportType")]
    [ApiController]
    public class SupportTypeController : Controller
    {
        private readonly ISupportTypeService _supportTypeService;

        public SupportTypeController(ISupportTypeService supportTypeService)
        {
            _supportTypeService = supportTypeService;
        }
        /// <summary>
        /// Get all SupportType
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportTypeReadDTO>>))]
        public async Task<IActionResult> GetAllSupportTypes()
        {
            var supportTypes = await _supportTypeService.GetAllLandTypeAsync();
            return ResponseFactory.Ok(supportTypes);
        }

        /// <summary>
        /// Get all actived SupportType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllActived")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportTypeReadDTO>>))]
        public async Task<IActionResult> getAllActivedSupportTypes()
        {
            var supportTypes = await _supportTypeService.GetAllActivedLandTypeAsync();
            return ResponseFactory.Ok(supportTypes);
        }

        /// <summary>
        /// Get SupportType
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        public async Task<IActionResult> GetSupportType(string id)
        {
            var supportType = await _supportTypeService.GetAsync(id);
            return ResponseFactory.Ok(supportType);
        }

        /// <summary>
        /// Create new SupportType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateSupportType(SupportTypeWriteDTO input)
        {
            var supportType = await _supportTypeService.CreateLandTypeAsync(input);
            return ResponseFactory.Created(supportType);
        }

        /// <summary>
        /// Create list SupportType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<SupportTypeReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateSupportTypes(IEnumerable<SupportTypeWriteDTO> input)
        {
            var supportTypes = await _supportTypeService.CreateLandTypesAsync(input);
            return ResponseFactory.Created(supportTypes);
        }

        /// <summary>
        /// Update SupportType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateSupportType(string id, SupportTypeWriteDTO writeDTO)
        {
            var supportType = await _supportTypeService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(supportType);
        }

        /// <summary>
        /// Delete SupportType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteSupportType(string id)
        {
            await _supportTypeService.DeleteAsync(id);
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
            await _supportTypeService.CheckNameSupportTypeNotDuplicate(name);
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
            await _supportTypeService.CheckCodeSupportTypeNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  SupportType
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportTypeReadDTO>>))]
        public async Task<IActionResult> QuerySupportType([FromQuery] SupportTypeQuery query)
        {
            var supportTypes = await _supportTypeService.QuerySupportTypeAsync(query);
            return ResponseFactory.Ok(supportTypes);
        }

        //import data from excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportSupportTypes(IFormFile file)
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
                await _supportTypeService.ImportSupportTypesFromExcelAsync(filePath);
                return Ok("Support types imported successfully");
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
