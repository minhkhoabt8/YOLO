using Document.Infrastructure.DTOs.Documemt;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Attributes;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Document.API.Controllers
{
    [Route("document")]
    [ApiController]
    public class DocumentController : Controller
    {
        
        /// <summary>
        /// Get documents
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(ApiPaginatedOkResponse<DocumentReadDTO>))]
        public async Task<IActionResult> QueryDocumentAsync([FromQuery] DocumentQuery query)
        {
            throw new NotImplementedException();
        }

       

        /// <summary>
        /// Create new  document 
        /// </summary>
        /// <returns></returns>
        
        [AuthorizeInternalService]
        [HttpPost()]
        [ServiceFilter(typeof(AutoValidateModelState))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentReadDTO>))]
        public async Task<IActionResult> CreateDocumentAsync(DocumentWriteDTO dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update document
        /// </summary>
        /// <returns></returns>
        [AuthorizeInternalService]
        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(AutoValidateModelState))]
        public async Task<IActionResult> UpdateDocumentAsync(int id, DocumentReadDTO dto)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// Delete a document
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}