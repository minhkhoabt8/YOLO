﻿using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Core.Exceptions;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Auth.API.Controllers;

[ApiController]
[Route("auth/login")]
[AllowAnonymous]
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
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    [ServiceFilter(typeof(AutoValidateModelState))]
    public async Task<IActionResult> Login(LoginInputDTO input)
    {
        var result = await _authService.LoginAsync(input);
        return ResponseFactory.Ok(result);
    }

    /// <summary>
    /// Reset Password Then Verify Account
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("reset")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    [ServiceFilter(typeof(AutoValidateModelState))]
    public async Task<IActionResult> FirstTimeResetPassWordAsync(ResetPasswordInputDTO input)
    {
        var result = await _authService.FirstTimeResetPasswordAsync(input);
        return ResponseFactory.Ok(result);
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginOutputDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    public async Task<IActionResult> Refresh(string? token)
    {
        string tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ","") ?? token;

        var result = await _authService.LoginWithRefreshTokenAsync(tokenHeader);

        return ResponseFactory.Ok(result);
    }

    /// <summary>
    /// Login with OTP
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost("otp-login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<LoginOutputDTO>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    public async Task<IActionResult> LoginWithOtp([Required] string userName, string? code)
    {
        var result = await _authService.LoginWithOtpAsync(userName, code);

        return ResponseFactory.Ok(result);
    }


    /// <summary>
    /// Resend OTP
    /// </summary>
    /// <returns></returns>
    [HttpPost("resend-otp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
    public async Task<IActionResult> ResendOtp(LoginInputDTO input)
    {
        var result = await _authService.ResendOtpAsync(input);

        return ResponseFactory.Ok(result);
    }

}