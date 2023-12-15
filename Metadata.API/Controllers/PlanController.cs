using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;
using System.ComponentModel.DataAnnotations;

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
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> QueryPlans([FromQuery] PlanQuery query)
        {
            var plans = await _planService.QueryPlanAsync(query);

            return ResponseFactory.PaginatedOk(plans);
        }

        /// <summary>
        /// Query Plans Of Creator
        /// </summary>
        /// <param name="query"></param>
        /// <param name="planStatus"></param>
        /// <returns></returns>
        [HttpGet("creator")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> QueryPlansOfCreatorAsync([FromQuery] PlanQuery query, PlanStatusEnum? planStatus = null)
        {
            var plans = await _planService.QueryPlansOfCreatorAsync(query, planStatus);

            return ResponseFactory.PaginatedOk(plans);
        }

        /// <summary>
        /// Query Plans Of Approval
        /// </summary>
        /// <param name="query"></param>
        /// <param name="planStatus"></param>
        /// <returns></returns>
        [HttpGet("approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> QueryPlansOfApprovalAsync([FromQuery] PlanQuery query, PlanStatusEnum? planStatus = null)
        {
            var plans = await _planService.QueryPlanOfApprovalAsync(query, planStatus);

            return ResponseFactory.PaginatedOk(plans);
        }


        /// <summary>
        /// Get Plan Details
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpGet("{planId}")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> GetPlanDetails(string planId)
        {
            var plan = await _planService.GetPlanAsync(planId);

            return ResponseFactory.Ok(plan);
        }

        /// <summary>
        /// Query Plans Of Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("project")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> QueryPlansOfProjectAsync([FromQuery] string? projectId, [FromQuery] PlanQuery query)
        {
            var plans = await _planService.QueryPlansOfProjectAsync(projectId, query);

            return ResponseFactory.PaginatedOk(plans);
        }

        /// <summary>
        /// Get ReCheck Prices Of Plan
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpGet("recheck")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        public async Task<IActionResult> ReCheckPricesOfPlanAsync([Required] string planId, bool applyChanged = false)
        {
            var plan = await _planService.ReCheckPricesOfPlanAsync(planId);

            return ResponseFactory.Ok(plan);
        }


        /// <summary>
        /// Create Plan
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreatePlanAsync(PlanWriteDTO dto)
        {
            var plan = await _planService.CreatePlanAsync(dto);

            return ResponseFactory.Created(plan);
        }

        /// <summary>
        /// Create Plan Copy
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpPost("create/copy")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreatePlanCopyAsync(string planId)
        {
            var plan = await _planService.CreatePlanCopyAsync(planId);

            return ResponseFactory.Created(plan);
        }


        /// <summary>
        /// Import Plan From File
        /// </summary>
        /// <param name="attachFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [Authorize(Roles = "Creator")]
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
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportProjectFile(string projectId)
        {
            var result = await _planService.ExportPlansFileAsync(projectId);
            return File(result.FileByte, result.FileType, result.FileName);
        }

        /// <summary>
        /// Export Phuong An Bao Cao File Report
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="filetype">default: .docx, 0: .pdf</param>
        /// <returns></returns>
        [HttpGet("export/planReport/{planId}")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportPlanReportsWordAsync([Required] string planId, FileTypeEnum filetype = FileTypeEnum.docx)
        {
            var result = await _planService.ExportPlanReportsWordAsync(planId, filetype);

            return File(result.FileByte, result.FileType, result.FileName);
        }


        /// <summary>
        /// Update Plan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
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
        /// Send Plan Approve Request
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpPut("request-approve")]
        [Authorize(Roles = "Creator")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> SendPlanApproveRequestAsync(string planId)
        {
            var plan = await _planService.SendPlanApproveRequestAsync(planId);

            return ResponseFactory.Ok(plan);
        }


        /// <summary>
        /// Approve Plan
        /// Must Use Corect Signer Token
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="signaturePassword"></param>
        /// <param name="signingFile">File need sign</param>
        /// <returns></returns>
        [HttpPut("approve")]
        [Authorize(Roles = "Approval")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ApprovePlanAsync([Required] string planId, [Required] string signaturePassword,[Required] IFormFile signingFile)
        {
            var plan = await _planService.ApprovePlanAsync(planId, signaturePassword, signingFile);

            return ResponseFactory.Ok(plan);
        }

        /// <summary>
        /// Approve Plan With Signed Document
        /// Must Use Corect Signer Token
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="signedFile">File that have signature</param>
        /// <returns></returns>
        [HttpPut("approve/signed")]
        [Authorize(Roles = "Approval")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> ApprovePlanWithSignedAsync([Required] string planId, [Required] IFormFile signedFile)
        {
            var plan = await _planService.ApprovePlanWithSignedDocumentAsync(planId, signedFile);

            return ResponseFactory.Ok(plan);
        }

        /// <summary>
        /// Reject Plan
        /// Must Use Corect Signer Token
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPut("reject")]
        [Authorize(Roles = "Approval")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<PlanReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> RejectPlanAsync([Required] string planId, [Required] string reason)
        {
            var plan = await _planService.RejectPlanAsync(planId, reason);

            return ResponseFactory.Ok(plan);
        }

        /// <summary>
        /// Delete Plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeletePlan(string id)
        {
            await _planService.DeletePlan(id);
            return ResponseFactory.NoContent();
        }


        //get bth chi phi
        [HttpGet("bthchiphi/{planId}")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DetailBTHChiPhiReadDTO>))]
        public async Task<IActionResult> getDataForBTHChiPhiAsync(string planId)
        {
            var plan = await _planService.getDataForBTHChiPhiAsync(planId);

            return ResponseFactory.Ok(plan);
        }

        // Bảng Tổng Hợp Chi Phí Report Excel
        [HttpGet("export/bthchiphi/{planId}")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportBTHChiPhiToExcelAsync(string planId, FileTypeEnum filetype = FileTypeEnum.xlsx)
        {
            var result = await _planService.ExportBTHChiPhiToExcelAsync(planId, filetype);

            return File(result.FileByte, result.FileType, result.FileName);
        }

        //ExportBTHThuHoiToExcelAsync
        [HttpGet("export/bththuhoi/{planId}")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportBTHThuHoiToExcelAsync(string planId, FileTypeEnum filetype = FileTypeEnum.xlsx)
        {
            var result = await _planService.ExportBTHThuHoiToExcelAsync(planId, filetype);

            return File(result.FileByte, result.FileType, result.FileName);
        }

        //api check not allow duplicate plan code
        [HttpGet("checknotallowduplicateplan/{planCode}")]
        [Authorize(Roles = "Creator,Approval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckNotAllowDuplicatePlanCode(string planCode)
        {
            var result = await _planService.CheckDuplicatePlanCodeAsync(planCode);

            return ResponseFactory.Ok(result);
        }
    }
}
