using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
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
    [ApiController]
    [Route("metadata/plan")]
    public class PlanController : ControllerBase
    {

        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        /// <summary>
        /// Query Plans
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> QueryPlans([FromQuery] PlanQuery query)
        {
            var plans = await _planService.QueryPlanAsync(query);

            return ResponseFactory.PaginatedOk(plans);
        }


        /// <summary>
        /// Get Plan Details
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpGet("{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> GetPlanDetails(string planId)
        {
            var plan = await _planService.GetPlanAsync(planId);

            return ResponseFactory.Ok(plan);
        }
        /// <summary>
        /// Get Plans Of Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("project")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> GetPlansOfProjectASync(string projectId)
        {
            var plans = await _planService.GetPlansOfProjectASync(projectId);

            return ResponseFactory.Ok(plans);
        }


        /// <summary>
        /// Create Plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreatePlanAsync(PlanWriteDTO dto)
        {
            var plan = await _planService.CreatePlanAsync(dto);

            return ResponseFactory.Created(plan);
        }

        /// <summary>
        /// Import Plan From File
        /// </summary>
        /// <param name="attachFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> ImportPlanAsync(IFormFile attachFile)
        {
            await _planService.ImportPlan(attachFile);
            return ResponseFactory.Created(attachFile);
        }
        /// <summary>
        /// Export Project File
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("export/test/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportProjectFile(string projectId)
        {
            var result = await _planService.ExportPlansFileAsync(projectId);
            return File(result.FileByte, result.FileType, result.FileName);
        }

        /// <summary>
        /// Export BTHT File Report (.docx)
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpGet("export/btht/{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportBthtFile(string planId)
        {
            var result = await _planService.ExportBTHTPlansWordAsync(planId);

            return File(result.FileByte, result.FileType, result.FileName);
        }


        /// <summary>
        /// Update Plan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdatePlan(string id, PlanWriteDTO writeDTO)
        {
            var owner = await _planService.UpdatePlanAsync(id, writeDTO);

            return ResponseFactory.Ok(owner);
        }

        /// <summary>
        /// Delete Plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeletePlan(string id)
        {
            await _planService.DeletePlan(id);
            return ResponseFactory.NoContent();
        }


        //get bth chi phi
        [HttpGet("bthchiphi/{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DetailBTHChiPhiReadDTO>))]
        public async Task<IActionResult> getDataForBTHChiPhiAsync(string planId)
        {
            var plan = await _planService.getDataForBTHChiPhiAsync(planId);

            return ResponseFactory.Ok(plan);
        }
    }
}
