﻿using Metadata.Infrastructure.DTOs.SupportType;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/supportType")]
    [ApiController]
    public class SupportTypeController : Controller
    {
        private readonly ISupportTypeService _supportTypeService;

        public SupportTypeController(ISupportTypeService supportTypeService)
        {
            _supportTypeService = supportTypeService;
        }
        /// <summary>
        /// Get all SupportType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<SupportTypeReadDTO>>))]
        public async Task<IActionResult> getAllSupportTypes()
        {
            var supportTypes = await _supportTypeService.GetAllLandTypeAsync();
            return ResponseFactory.Ok(supportTypes);
        }

        /// <summary>
        /// Get SupportType
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        public async Task<IActionResult> getSupportType(string id)
        {
            var supportType = await _supportTypeService.GetAsync(id);
            return ResponseFactory.Ok(supportType);
        }

        /// <summary>
        /// Create new SupportType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<SupportTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateSupportType(SupportTypeWriteDTO input)
        {
            var supportType = await _supportTypeService.CreateLandTypeAsync(input);
            return ResponseFactory.Created(supportType);
        }

        /// <summary>
        /// Update SupportType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete SupportType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
