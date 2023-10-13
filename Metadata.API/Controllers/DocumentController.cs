﻿using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
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
    [Route("metadata/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Query Documents
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<OwnerReadDTO>))]
        public async Task<IActionResult> QueryOwners([FromQuery] DocumentQuery query)
        {
            var owners = await _documentService.QueryDocumentAsync(query);
            return ResponseFactory.PaginatedOk(owners);
        }

        /// <summary>
        /// Create Document
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateDocumentAsync([FromForm] DocumentWriteDTO dtos)
        {

            var documents = await _documentService.CreateDocumentAsync(dtos);

            return ResponseFactory.Created(documents);
        }

        /// <summary>
        /// Update Document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<OwnerReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateDocument(string id, DocumentWriteDTO writeDTO)
        {
            var document = await _documentService.UpdateDocumentAsync(id, writeDTO);
            return ResponseFactory.Ok(document);
        }

        /// <summary>
        /// Delete Document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
