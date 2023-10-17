using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("metadata/documentType")]
    [ApiController]
    public class DocumentTypeController : Controller
    {
       private readonly IDocumentTypeService _documentTypeService;

        public DocumentTypeController(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        /// <summary>
        /// Get all DocumentTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DocumentTypeReadDTO>>))]
        public async Task<IActionResult> getAllDocumentTypes()
        {
            var documentTypes = await _documentTypeService.GetAllDocumentTypesAsync();
            return ResponseFactory.Ok(documentTypes);
        }

        /// <summary>
        /// Get DocumentTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("getById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        public async Task<IActionResult> getDocumentType(string id)
        {
            var documentType = await _documentTypeService.GetDocumentTypeAsync(id);
            return ResponseFactory.Ok(documentType);
        }

        /// <summary>
        /// Create new DocumentTypes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateDocumentType(DocumentTypeWriteDTO input)
        {
            var documentType = await _documentTypeService.CreateDocumentTypeAsync(input);
            return ResponseFactory.Created(documentType);
        }

        /// <summary>
        /// Update DocumentType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("UpdateId")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UpdateDocumentType(string id, DocumentTypeWriteDTO writeDTO)
        {
            var documentType = await _documentTypeService.UpdateDocumentTypeAsync(id, writeDTO);
            return ResponseFactory.Ok(documentType);
        }

        /// <summary>
        /// Delete DocumentType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteDocumentType(string id)
        {
            await _documentTypeService.DeleteDocumentTypeAsync(id);
            return ResponseFactory.NoContent();
        }
    }
}
