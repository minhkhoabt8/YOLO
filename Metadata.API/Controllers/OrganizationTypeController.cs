using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/organizationType")]
    [ApiController]
    public class OrganizationTypeController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationTypeController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        /// <summary>
        /// Get all OrganizationType
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        public async Task<IActionResult> GetAllOrganizationTypes()
        {
            var organizationTypes = await _organizationService.GetAllOrganizationTypeAsync();
            return ResponseFactory.Ok(organizationTypes);
        }

        /// <summary>
        /// Get OrganizationType
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        public async Task<IActionResult> GetOrganizationType(string id)
        {
            var organizationType = await _organizationService.GetAsync(id);
            return ResponseFactory.Ok(organizationType);
        }

        /// <summary>
        /// Get all actived OrganizationType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllActivedOrganizationType")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        public async Task<IActionResult> getAllActivedOrganizationType()
        {
            var organizationTypes = await _organizationService.GetAllActivedOrganizationTypeAsync();
            return ResponseFactory.Ok(organizationTypes);
        }


        /// <summary>
        /// Create new OrganizationType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateOrganizationType(OrganizationTypeWriteDTO input)
        {
            var organizationType = await _organizationService.CreateOrganizationTypeAsync(input);
            return ResponseFactory.Created(organizationType);
        }

        /// <summary>
        /// Create new OrganizationType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateOrganizationTypeList(IEnumerable<OrganizationTypeWriteDTO> input)
        {
            var organizationType = await _organizationService.CreateOrganizationTypesAsync(input);
            return ResponseFactory.Created(organizationType);
        }

        /// <summary>
        /// Update OrganizationType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateOrganizationType(string id, OrganizationTypeWriteDTO writeDTO)
        {
            var organizationType = await _organizationService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(organizationType);
        }

        /// <summary>
        /// Delete OrganizationType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteOrganizationType(string id)
        {
            await _organizationService.DeleteAsync(id);
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
             await _organizationService.CheckNameOrganizationTypeNotDuplicate(name);
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
            await _organizationService.CheckCodeOrganizationTypeNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  OrganizationType
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        public async Task<IActionResult> QueryOrganizationType([FromQuery] OrganizationTypeQuery query)
        {
            var organizationTypes = await _organizationService.QueryOrganizationTypeAsync(query);
            return ResponseFactory.Ok(organizationTypes);
        }

        //import data from excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportOrganizationTypes(IFormFile file)
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
                await _organizationService.ImportOrganizationTypeFromExcelAsync(filePath);
                return Ok("Organization type imported successfully");
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
