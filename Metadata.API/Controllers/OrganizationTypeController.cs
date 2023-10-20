using Metadata.Infrastructure.DTOs.OrganizationType;
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
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        public async Task<IActionResult> getAllOrganizationTypes()
        {
            var organizationTypes = await _organizationService.GetAllOrganizationTypeAsync();
            return ResponseFactory.Ok(organizationTypes);
        }

        /// <summary>
        /// Get OrganizationType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        public async Task<IActionResult> getOrganizationType(string id)
        {
            var organizationType = await _organizationService.GetAsync(id);
            return ResponseFactory.Ok(organizationType);
        }

        /// <summary>
        /// Get all deleted OrganizationType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllDeletedOrganizationType")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        public async Task<IActionResult> getAllDeletedOrganizationType()
        {
            var organizationTypes = await _organizationService.GetAllDeletedOrganizationTypeAsync();
            return ResponseFactory.Ok(organizationTypes);
        }


        /// <summary>
        /// Create new OrganizationType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateOrganizationType(OrganizationTypeWriteDTO input)
        {
            var organizationType = await _organizationService.CreateOrganizationTypeAsync(input);
            return ResponseFactory.Created(organizationType);
        }

        /// <summary>
        /// Update OrganizationType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("UpdateId")]
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
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteOrganizationType(string id)
        {
            await _organizationService.DeleteAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
