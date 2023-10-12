using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/project")]
    [ApiController]
    public class OrganizationTypeController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationTypeController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OrganizationTypeReadDTO>>))]
        public async Task<IActionResult> getAllOrganizationTypes()
        {
            var organizationTypes = await _organizationService.GetAllOrganizationTypeAsync();
            return ResponseFactory.Ok(organizationTypes);
        }

        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateOrganizationType(OrganizationTypeWriteDTO writeDTO)
        {
            var organizationType = await _organizationService.CreateOrganizationTypeAsync(writeDTO);
            return ResponseFactory.Created(organizationType);
        }

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

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OrganizationTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteOrganizationType(string ob)
        {
            await _organizationService.DeleteAsync(ob);
            return ResponseFactory.NoContent();
        }
    }
}
