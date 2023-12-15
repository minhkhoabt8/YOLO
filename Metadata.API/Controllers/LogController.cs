using Metadata.Infrastructure.DTOs.AuditTrail;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/log")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IAuditTrailService _auditTrailService;

        public LogController(IAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }

        /// <summary>
        /// Query Logs
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<AuditTrailReadDTO>))]
        public async Task<IActionResult> QueryLogs([FromQuery] AuditTrailQuery query)
        {
            var projects = await _auditTrailService.QueryAuditsAsync(query);

            return ResponseFactory.PaginatedOk(projects);
        }
    }
}
