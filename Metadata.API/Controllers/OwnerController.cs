using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SharedLib.Filters;
using SharedLib.Infrastructure.DTOs;
using SharedLib.ResponseWrapper;
using System.ComponentModel.DataAnnotations;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("metadata/owner")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        /// <summary>
        /// Query Owners
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<OwnerReadDTO>))]
        public async Task<IActionResult> QueryOwners([FromQuery] OwnerQuery query)
        {
            var owners = await _ownerService.QueryOwnerAsync(query);

            return ResponseFactory.PaginatedOk(owners);
        }


        /// <summary>
        /// Get Owner Details
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        [HttpGet("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        public async Task<IActionResult> GetOwnerDetails(string ownerId)
        {
            var owner = await _ownerService.GetOwnerAsync(ownerId);

            return ResponseFactory.Ok(owner);
        }

        /// <summary>
        /// Get All Owners Of Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<OwnerReadDTO>>))]
        public async Task<IActionResult> GetOwnersOfProjectAsync(string projectId)
        {
            var owner = await _ownerService.GetOwnersOfProjectAsync(projectId);

            return ResponseFactory.Ok(owner);
        }


        /// <summary>
        /// Get Owners In Plan By PlanId And Owners In Project That Not In Any Plan By Project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="planId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("plan/{planId}/project/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PaginatedResponse<OwnerReadDTO>>))]
        public async Task<IActionResult> GetOwnerInPlanByPlanIdAndOwnerInProjectThatNotInAnyPlanByProjectIdAsync([FromQuery] PaginatedQuery query, string planId, string projectId)
        {
            var owner = await _ownerService.GetOwnerInPlanByPlanIdAndOwnerInProjectThatNotInAnyPlanByProjectIdAsync(query,planId, projectId);

            return ResponseFactory.Ok(owner);
        }


        /// <summary>
        /// Create Owner
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateOwnerWithFullInfomationAsync(OwnerWriteDTO dto)
        {
            
            var owner = await _ownerService.CreateOwnerWithFullInfomationAsync(dto);

            return ResponseFactory.Created(owner);
        }

        /// <summary>
        /// Assign Project To Owner
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost("assign/project")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> AssignProjectOwnerAsync([Required][FromQuery] string ownerId, [Required][FromQuery] string projectId)
        {
            var owner = await _ownerService.AssignProjectOwnerAsync(projectId, ownerId);

            return ResponseFactory.Created(owner);
        }

        /// <summary>
        /// Assign Plan To Owners
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="ownerIds"></param>
        /// <returns></returns>
        [HttpPost("assign/plan")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<OwnerReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> AssignPlanToOwnerAsync([Required] string planId, [Required] IEnumerable<string> ownerIds)
        {
            var owners = await _ownerService.AssignPlanToOwnerAsync(planId, ownerIds);

            return ResponseFactory.Created(owners);
        }


        /// <summary>
        /// Remove Owner From Plan
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="ownerIds"></param>
        /// <returns></returns>
        [HttpPost("remove/plan")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> RemoveOwnerFromPlanAsync([Required][FromQuery] string planId, [Required] IEnumerable<string> ownerIds)
        {
            var owner = await _ownerService.RemoveOwnerFromPlanAsync(planId, ownerIds);

            return ResponseFactory.Created(owner);
        }

        /// <summary>
        /// Remove Owner From Project
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost("project/remove")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> RemoveOwnerFromProjectAsync([Required][FromQuery] string ownerId, [Required][FromQuery] string projectId)
        {
            var owner = await _ownerService.RemoveOwnerFromProjectAsync(ownerId, projectId);

            return ResponseFactory.Created(owner);
        }



        /// <summary>
        /// Import Owner From File
        /// </summary>
        /// <param name="attachFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> ImportOwnerAsync([Required] IFormFile file)
        {
            var result = await _ownerService.ImportOwnerFromExcelFileAsync(file);
            return ResponseFactory.Created(result);
        }

        /// <summary>
        /// Export Owner File
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("export/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportOwnerFile(string projectId)
        {
            var result = await _ownerService.ExportOwnerFileAsync(projectId);
            return File(result.FileByte, result.FileType, result.FileName);
        }

        /// <summary>
        /// Update Owner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateOwner(string id, OwnerWriteDTO writeDTO)
        {
            var owner = await _ownerService.UpdateOwnerAsync(id, writeDTO);
            return ResponseFactory.Ok(owner);
        }

        /// <summary>
        /// Delete Owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteOwner(string id)
        {
            await _ownerService.DeleteOwner(id);
            return ResponseFactory.NoContent();
        }
    }
}
