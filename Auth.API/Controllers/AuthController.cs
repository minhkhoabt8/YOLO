
using Auth.Infracstructure.DTOs.Account;
using Auth.Infracstructure.Services.Implementations;
using Auth.Infracstructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.ResponseWrapper;

namespace Auth.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISampleServices _sampleServices;
    private readonly IAccountService _accountService;

    public AuthController(ISampleServices sampleServices, IAccountService accountService)
    {
        _sampleServices = sampleServices;
        _accountService = accountService;
    }




    /// <summary>
    /// Get Sample
    /// </summary>
    /// <returns></returns>
    [HttpGet("sample")]
    public async Task<IActionResult> GetSample()
    {
        
        var result = _sampleServices.GetSampleInfo();

        return Ok(result);
    }

    /// <summary>
    /// Get all accounts
    /// </summary>
    /// <returns></returns>
    [HttpGet("account/all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AccountReadDTO>>))]
    public async Task<IActionResult> GetAll()
    {
        var accountDTOs = await _accountService.GetAllAccountsAsync();

        return ResponseFactory.Ok(accountDTOs);
    }
}