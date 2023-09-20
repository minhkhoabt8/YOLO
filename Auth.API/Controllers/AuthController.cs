using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Core.Exceptions;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Auth.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Login
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginOutputDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    [ServiceFilter(typeof(AutoValidateModelState))]
    public async Task<IActionResult> Login(LoginInputDTO input)
    {
        var result = await _authService.LoginAsync(input);
        return ResponseFactory.Ok(result);
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginOutputDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    [ServiceFilter(typeof(AutoValidateModelState))]
    public async Task<IActionResult> Refresh(string? token)
    {

        var result = await _authService.LoginWithRefreshTokenAsync(token);

        return ResponseFactory.Ok(result);
    }
}