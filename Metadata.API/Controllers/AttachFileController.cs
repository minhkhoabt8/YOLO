using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.AttachFile;
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
    [Route("metadata/attachFile")]
    [ApiController]
    public class AttachFileController : ControllerBase
    {
        private readonly IAttachFileService _attachFileService;

        public AttachFileController(IAttachFileService attachFileService)
        {
            _attachFileService = attachFileService;
        }

        /// <summary>
        /// Get All Attach File
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AttachFileReadDTO>>))]
        public async Task<IActionResult> GetAllAttachFilesAsync()
        {
            var attachFiles = await _attachFileService.GetAllAttachFileAsync();
            return ResponseFactory.Ok(attachFiles);
        }


        /// <summary>
        /// Get Attach File Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AttachFileReadDTO>))]
        public async Task<IActionResult> GetAttachFileDetailAsync(string id)
        {
            var attachFile = await _attachFileService.GetAttachFileDetailsAsync(id);
            return ResponseFactory.Ok(attachFile);
        }

        /// <summary>
        /// Create Attach Files
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AttachFileReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateAttachFilesAsync(IEnumerable<AttachFileWriteDTO> dtos)
        {
            var attachFiles = await _attachFileService.CreateAttachFilesAsync(dtos);
            return ResponseFactory.Created(attachFiles);
        }

        /// <summary>
        /// Update Attach File
        /// </summary>
        /// <param name="attachFileId"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AssetUnitReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateAttachFileAsync(string attachFileId, AttachFileWriteDTO writeDTO)
        {
            var attachFile = await _attachFileService.UpdateAttachFileAsync(attachFileId, writeDTO);
            return ResponseFactory.Ok(attachFile);
        }


        /// <summary>
        /// Delete Attach File
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AttachFileReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteAttachFilesAsync(string id)
        {
            await _attachFileService.DeleteAttachFileAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
