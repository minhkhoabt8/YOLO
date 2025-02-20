﻿using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.Infrastructure.DTOs;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<DocumentReadDTO>))]
        public async Task<IActionResult> QueryDocuments([FromQuery] DocumentQuery query)
        {
            var owners = await _documentService.QueryDocumentAsync(query);
            return ResponseFactory.PaginatedOk(owners);
        }

        /// <summary>
        /// Get Documents Of Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("projectId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentReadDTO>))]
        public async Task<IActionResult> GetDocumentsOfProject(string projectId)
        {
            var documents = await _documentService.GetDocumentsOfProjectAsync(projectId);
            return ResponseFactory.Ok(documents);
        }

        /// <summary>
        /// Check Duplicate Document
        /// </summary>
        /// <param name="number"></param>
        /// <param name="notation"></param>
        /// <param name="epitome"></param>
        /// <returns></returns>
        [HttpGet("duplicate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentReadDTO>))]
        public async Task<IActionResult> CheckDuplicateDocumentAsync(int number, string notation, string epitome)
        {
            var document = await _documentService.CheckDuplicateDocumentAsync(number, notation, epitome);
            return ResponseFactory.Ok(document);
        }


        /// <summary>
        /// Get Excel File Import Template
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadFileDTO))]
        public async Task<IActionResult> GetFileImportExcelTemplateAsync(string name)
        {
            var result = await _documentService.GetFileImportExcelTemplateAsync(name);
            return File(result.FileByte, result.FileType, result.FileName);
        }



        /// <summary>
        /// Create Document
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentReadDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        public async Task<IActionResult> CreateDocumentAsync(DocumentWriteDTO dtos)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentReadDTO>))]
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
