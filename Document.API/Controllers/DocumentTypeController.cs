
using Document.Infrastructure.DTOs.DocumentType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Document.API.Controllers;

[Authorize]
[Route("document/types")]
[ApiController]
public class DocumentTypeController : ControllerBase
{
    

    /// <summary>
    /// Get all document types
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<DocumentTypeReadDTO>>))]
    public async Task<IActionResult> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create new document type
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
    public async Task<IActionResult> CreateAsync(DocumentTypeWriteDTO dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update document type
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{id}")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<DocumentTypeReadDTO>))]
    public async Task<IActionResult> UpdateAsync(int id, DocumentTypeWriteDTO dto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete document type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
       throw new NotImplementedException();
    }
}