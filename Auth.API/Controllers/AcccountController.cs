using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Filters;
using SharedLib.ResponseWrapper;

namespace Auth.API.Controllers;

[ApiController]
[Route("auth/account")]
public class AcccountController : ControllerBase
{

    private readonly IAccountService _accountService;

    public AcccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Get all accounts
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<IEnumerable<AccountReadDTO>>))]
    public async Task<IActionResult> GetAll()
    {
        var accountDTOs = await _accountService.GetAllAccountsAsync();

        return ResponseFactory.Ok(accountDTOs);
    }


    /// <summary>
    /// Query accounts
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<AccountReadDTO>))]
    public async Task<IActionResult> QueryAccounts([FromQuery] AccountQuery query)
    {
       throw new NotImplementedException();
    }

    /// <summary>
    /// Get account
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> GetAccount(string id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create new account
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    public async Task<IActionResult> CreateAccount([FromForm] AccountWriteDTO input)
    {
        var accountDTOs = await _accountService.CreateAccountAsync(input);

        return ResponseFactory.Created(accountDTOs);
    }
}
