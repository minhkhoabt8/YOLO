using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("all")]
        [Authorize(Roles = "Creator,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DocumentTypeReadDTO>>))]
        public async Task<IActionResult> GetAllDocumentTypes()
        {
            var documentTypes = await _documentTypeService.GetAllDocumentTypesAsync();
            return ResponseFactory.Ok(documentTypes);
        }

        /// <summary>
        /// Get All Actived DocumentTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("getActived")]
        [Authorize(Roles = "Creator,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DocumentTypeReadDTO>>))]
        public async Task<IActionResult> getAllActiveDocumentType()
        {
            var documentTypes = await _documentTypeService.GetAllActivedDocumentTypes();
            return ResponseFactory.Ok(documentTypes);
        }

        /// <summary>
        /// Get DocumentTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Creator,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        public async Task<IActionResult> GetDocumentType(string id)
        {
            var documentType = await _documentTypeService.GetDocumentTypeAsync(id);
            return ResponseFactory.Ok(documentType);
        }

        /// <summary>
        /// Create new DocumentTypes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Creator,Admin")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateDocumentType(DocumentTypeWriteDTO input)
        {
            var documentType = await _documentTypeService.CreateDocumentTypeAsync(input);
            return ResponseFactory.Created(documentType);
        }

        /// <summary>
        /// CreateListDocumentType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [Authorize(Roles = "Creator,Admin")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<IEnumerable<DocumentTypeReadDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
        public async Task<IActionResult> CreateListDocumentType(IEnumerable<DocumentTypeWriteDTO> input)
        {
            var documentType = await _documentTypeService.CreateListDocumentTypeAsync(input);
            return ResponseFactory.Created(documentType);
        }

        /// <summary>
        /// Update DocumentType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="writeDTO"></param>
        /// <returns></returns>
        [HttpPut("updateId")]
        [Authorize(Roles = "Creator,Admin")]
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
        [HttpDelete("delete")]
        [Authorize(Roles = "Creator,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> DeleteDocumentType(string id)
        {
            await _documentTypeService.DeleteDocumentTypeAsync(id);
            return ResponseFactory.NoContent();
        }

        /// <summary>
        /// Check Duplicate Name
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateName")]
        [Authorize(Roles = "Creator,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateName(string name)
        {
             await _documentTypeService.CheckNameDocumentTypeNotDuplicate(name);
            return ResponseFactory.Accepted();
        }
        /// <summary>
        /// Check Duplicate Code
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkDuplicateCode")]
        [Authorize(Roles = "Creator,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<bool>))]
        public async Task<IActionResult> CheckDuplicateCode(string code)
        {
             await _documentTypeService.CheckCodeDocumentTypeNotDuplicate(code);
            return ResponseFactory.Accepted();
        }

        /// <summary>
        /// Query  DocumentType
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("query")]
        [Authorize(Roles = "Creator,Approval,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DocumentTypeReadDTO>>))]
        public async Task<IActionResult> QueryDocumentType([FromQuery] DocumentTypeQuery query)
        {
            var documentTypes = await _documentTypeService.QueryDocumentTypeAsync(query);
            return ResponseFactory.PaginatedOk(documentTypes);
        }


        /// <summary>
        /// import data from excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [Authorize(Roles = "Creator,Admin")]
        public async Task<IActionResult> ImportDocumentType(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string filePath = Path.GetTempFileName();

            // Save the uploaded file to a temporary file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                var dataImport = await _documentTypeService.ImportDocumenTypeFromExcelAsync(filePath);
                return Ok(new { Message = "Document types imported successfully", Data = dataImport });
               
            }
            catch (Exception ex)
            {

                return StatusCode(500, $" {ex.Message}");
            }
            finally
            {

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
