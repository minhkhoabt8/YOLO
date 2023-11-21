using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("metadata/resettlementProject")]
    [ApiController]
    public class ResettlementProjectController : ControllerBase
    {
        private readonly IResettlementProjectService _resettlementProjectService;

        public ResettlementProjectController(IResettlementProjectService resettlementProjectService)
        {
            _resettlementProjectService = resettlementProjectService;
        }

        /// <summary>
        /// Query Resettlement
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<ResettlementProjectReadDTO>))]
        public async Task<IActionResult> QueryResettlementProjectsAsync([FromQuery] ResettlementProjectQuery query)
        {
            var resettlements = await _resettlementProjectService.ResettlementProjectQueryAsync(query);
            return ResponseFactory.PaginatedOk(resettlements);
        }

        /// <summary>
        /// Get Resettlement Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ResettlementProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetResettlementProjectsAsync(string id)
        {
            var resettlement = await _resettlementProjectService.GetResettlementProjectAsync(id);
            return ResponseFactory.Ok(resettlement);
        }

        /// <summary>
        /// Get Resettlement Of A Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("project")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ResettlementProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetResettlementProjectByProjectIdAsync(string projectId)
        {
            var resettlement = await _resettlementProjectService.GetResettlementProjectByProjectIdAsync(projectId);
            return ResponseFactory.Ok(resettlement);
        }

        /// <summary>
        /// Create New Resettlement
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<ResettlementProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateResettlementProjectAsync(ResettlementProjectWriteDTO writeDTO)
        {
            var resettlement = await _resettlementProjectService.CreateResettlementProjectAsync(writeDTO);

            return ResponseFactory.Created(resettlement);
        }

        /// <summary>
        /// Create New Resettlement Document
        /// </summary>
        /// <param name="resettlementId"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        [HttpPost("create/document")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<ResettlementProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateResettlementProjectDocumentAsync(string resettlementId, IEnumerable<DocumentWriteDTO> documents)
        {
            var resettlement = await _resettlementProjectService.CreateResettlementProjectDocumentsAsync(resettlementId, documents);

            return ResponseFactory.Created(resettlement);
        }



        /// <summary>
        /// Update Resettlement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ResettlementProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateResettlementProjectAsync(string id, ResettlementProjectWriteDTO writeDTO)
        {
            var resettlement = await _resettlementProjectService.UpdateResettlementProjectAsync(id, writeDTO);
            return ResponseFactory.Ok(resettlement);
        }

        /// <summary>
        /// Delete Resettlement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<ResettlementProjectReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteResettlementProjectAsync(string id)
        {
            await _resettlementProjectService.DeleteResettlementProjectAsync(id);

            return ResponseFactory.NoContent();
        }

    }
}
