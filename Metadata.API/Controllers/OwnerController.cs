using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SharedLib.Filters;
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
        [HttpPost("assign/{ownerId}/{projectId}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> AssignProjectOwnerAsync([Required] string ownerId, [Required] string projectId)
        {
            var owner = await _ownerService.AssignProjectOwnerAsync(projectId, ownerId);

            return ResponseFactory.Created(owner);
        }

        /// <summary>
        /// Import Owner From File - Test Template
        /// </summary>
        /// <param name="attachFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> ImportOwnerAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file.");
            }
            var importedUsers = new List<UserTestClass>();
            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var user = new UserTestClass
                    {
                        UserId = worksheet.Cells[row, 1].Value.ToString(),    
                        UserName = worksheet.Cells[row, 2].Value.ToString(),       
                        Password = worksheet.Cells[row, 3].Value.ToString(),         
                        Role = worksheet.Cells[row, 4].Value.ToString()         
                    };
                    importedUsers.Add(user);
                }

                return Ok(importedUsers);
            }
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
