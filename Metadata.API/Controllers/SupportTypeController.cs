using Metadata.Infrastructure.DTOs.SupportType;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/project")]
    [ApiController]
    public class SupportTypeController : Controller
    {
        private readonly ISupportTypeService _supportTypeService;

        public SupportTypeController(ISupportTypeService supportTypeService)
        {
            _supportTypeService = supportTypeService;
        }
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportTypeReadDTO>>))]
        public async Task<IActionResult> getAllSupportTypes()
        {
            var supportTypes = await _supportTypeService.GetAllLandTypeAsync();
            return ResponseFactory.Ok(supportTypes);
        }

        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateSupportType(SupportTypeWriteDTO writeDTO)
        {
            var supportType = await _supportTypeService.CreateLandTypeAsync(writeDTO);
            return ResponseFactory.Created(supportType);
        }

        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateSupportType(string id, SupportTypeWriteDTO writeDTO)
        {
            var supportType = await _supportTypeService.UpdateAsync(id, writeDTO);
            return ResponseFactory.Ok(supportType);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteSupportType(string id)
        {
            await _supportTypeService.DeleteAsync(id);
            return ResponseFactory.NoContent();
        }

    }
}
