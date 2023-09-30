using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("metadata/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Query projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<ProjectReadDTO>))]
        public async Task<IActionResult> QueryAccounts([FromQuery] ProjectQuery query)
        {
            var projects = await _projectService.ProjectQueryAsync(query);

            return ResponseFactory.PaginatedOk(projects);
        }

        /// <summary>
        /// Create Project
        /// </summary>
        /// <param name="projectDto"></param>
        /// <param name="fileAttaches"></param>
        /// <returns></returns>
        [HttpPost()]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateProjectAsync([FromForm] ProjectWriteDTO projectDto)
        {
            var project = await _projectService.CreateProjectAsync(projectDto);

            return ResponseFactory.Created(project);
        }
    }
}
