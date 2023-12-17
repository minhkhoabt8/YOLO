using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Auth.API.Controllers;

[ApiController]
[Route("auth/role")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }


    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<RoleReadDTO>>))]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return ResponseFactory.Ok(roles);
    }


    /// <summary>
    /// Get role detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetRoleDetailsAsync(string id)
    {
        var role = await _roleService.GetRoleAsync(id);
        return ResponseFactory.Ok(role);
    }


    /// <summary>
    /// Create new role
    /// </summary>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    public async Task<IActionResult> CreateRole(RoleWriteDTO writeDTO)
    {
        var role = await _roleService.CreateRoleAsync(writeDTO);
        return ResponseFactory.Created(role);
    }



    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<RoleReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> UpdateRole(string id, RoleWriteDTO writeDTO)
    {
        var role = await _roleService.UpdateRoleAsync(id, writeDTO);
        return ResponseFactory.Ok(role);
    }


    /// <summary>
    /// Delete role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> DeleteRole(string id)
    {
        await _roleService.DeleteRoleAsync(id);
        return ResponseFactory.NoContent();
    }

}
