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
        /// Query Projects
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<ProjectReadDTO>))]
        public async Task<IActionResult> QueryProjects([FromQuery] ProjectQuery query)
        {
            var projects = await _projectService.ProjectQueryAsync(query);

            return ResponseFactory.PaginatedOk(projects);
        }


        /// <summary>
        /// Get Project Details
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ProjectReadDTO>))]
        public async Task<IActionResult> GetProjectDetails(string projectId)
        {
            var projects = await _projectService.GetProjectAsync(projectId);

            return ResponseFactory.Ok(projects);
        }

        /// <summary>
        /// Export Project File
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportProjectFile()
        {
            var result = await _projectService.ExportProjectFileAsync();
            return File(result.FileByte, result.FileType, result.FileName);
        }

        /// <summary>
        /// Create Project
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<ProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateProjectAsync(ProjectWriteDTO projectDto)
        {
            var project = await _projectService.CreateProjectAsync(projectDto);

            return ResponseFactory.Created(project);
        }


        /// <summary>
        /// Create Project Document
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        [HttpPost("create/document")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<ProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateProjectDocumentAsync(string projectId, IEnumerable<DocumentWriteDTO> documents)
        {
            var project = await _projectService.CreateProjectDocumentsAsync(projectId, documents);

            return ResponseFactory.Created(project);
        }


        /// <summary>
        /// Import Project From File
        /// </summary>
        /// <param name="attachFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<ProjectReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public Task<IActionResult> ImportProjectAsync(IFormFile attachFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateProject(string id, ProjectWriteDTO writeDTO)
        {
            var project = await _projectService.UpdateProjectAsync(id, writeDTO);
            return ResponseFactory.Ok(project);
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteProject(string id)
        {
            await _projectService.DeleteProjectAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
