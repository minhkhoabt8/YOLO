﻿using Metadata.Infrastructure.DTOs.LandResettlement;
using Metadata.Infrastructure.DTOs.ResettlementProject;
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
    [Route("metadata/landResettlement")]
    [ApiController]
    public class LandResettlementController : ControllerBase
    {
        private readonly ILandResettlementService _landResettlementService;

        public LandResettlementController(ILandResettlementService landResettlementService)
        {
            _landResettlementService = landResettlementService;
        }

        /// <summary>
        /// Get Land Resettlement Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandResettlementReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetLandResettlementsAsync(string id)
        {
            var resettlement = await _landResettlementService.GetLandResettlementAsync(id);
            return ResponseFactory.Ok(resettlement);
        }


        /// <summary>
        /// Get Land Resettlement Of Owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("owner")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandResettlementReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetLandResettlementsOfOwnerAsync(string ownerId)
        {
            var resettlements = await _landResettlementService.GetLandResettlementsOfOwnerAsync(ownerId);
            return ResponseFactory.Ok(resettlements);
        }

        /// <summary>
        /// Get Land Resettlement Of Owner
        /// </summary>
        /// <param name="resettlementProjectId"></param>
        /// <returns></returns>
        [HttpGet("resettlementProject")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<LandResettlementReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> GetLandResettlementsOfResettlementProjectAsync(string resettlementProjectId)
        {
            var resettlements = await _landResettlementService.GetLandResettlementsOfOwnerAsync(resettlementProjectId);
            return ResponseFactory.Ok(resettlements);
        }


        /// <summary>
        /// Create New Land Resettlement
        /// </summary>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<LandResettlementReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateLandResettlementAsync(LandResettlementWriteDTO writeDTO)
        {
            var resettlement = await _landResettlementService.CreateLandResettlementAsync(writeDTO);

            return ResponseFactory.Created(resettlement);
        }


        /// <summary>
        /// Update Land Resettlement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandResettlementReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateLandResettlementAsync(string id, LandResettlementWriteDTO writeDTO)
        {
            var resettlement = await _landResettlementService.UpdateLandResettlementAsync(id, writeDTO);
            return ResponseFactory.Ok(resettlement);
        }


        /// <summary>
        /// Delete Land Resettlement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LandResettlementReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteResettlementProjectAsync(string id)
        {
            await _landResettlementService.DeleteLandResettlementAsync(id);

            return ResponseFactory.NoContent();
        }
    }
}