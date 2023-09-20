using Auth.Infrastructure.DTOs.Role;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Auth.API.Controllers;

[ApiController]
[Route("auth/role")]
public class RoleController : ControllerBase
{
    
    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<RoleReadDTO>>))]
    public async Task<IActionResult> GetAllRoles()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Get role detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetRole(int id)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Create new role
    /// </summary>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    public async Task<IActionResult> CreateRole(RoleWriteDTO writeDTO)
    {
        throw new NotImplementedException();
    }



    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> UpdateRole(int id, RoleWriteDTO writeDTO)
    {

        throw new NotImplementedException();
    }


    /// <summary>
    /// Delete role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> DeleteRole(int id)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Assign roles to account
    /// </summary>
    /// <param name="accountID"></param>
    /// <param name="roleIDs"></param>
    /// <returns></returns>
    [HttpPut("/auth/accounts/{accountID:guid}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> AssignRolesToAccount(string accountID, int[]? roleIDs)
    {
        throw new NotImplementedException();
    }
}
