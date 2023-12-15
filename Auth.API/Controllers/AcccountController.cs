using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Implementations;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
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
    [HttpGet("query")]
    [Authorize(Roles = "Admin,Approval,Creator")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedOkResponse<AccountReadDTO>))]
    public async Task<IActionResult> QueryAccounts([FromQuery] AccountQuery query)
    {
        var accounts = await _accountService.QueryAccount(query);
        return ResponseFactory.PaginatedOk(accounts);
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
        var account = await _accountService.GetAccountByIdAsync(id);
        return ResponseFactory.Ok(account);
    }

    /// <summary>
    /// Create new account
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    public async Task<IActionResult> CreateAccount(AccountWriteDTO input)
    {
        var accountDTOs = await _accountService.CreateAccountAsync(input);

        return ResponseFactory.Created(accountDTOs);
    }

    /// <summary>
    /// Update account
    /// </summary>
    /// <param name="id"></param>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<AccountReadDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> UpdateAccount(string id, AccountUpdateDTO writeDTO)
    {
        var account = await _accountService.UpdateAccountAsync(id, writeDTO);
        return ResponseFactory.Ok(account);
    }

    /// <summary>
    /// Update account password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="writeDTO"></param>
    /// <returns></returns>
    [HttpPut("password")]
    [ServiceFilter(typeof(AutoValidateModelState))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiBadRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> UpdateAccountPassword(ResetPasswordInputDTO writeDTO)
    {
        await _accountService.UpdatePasswordAsync(writeDTO);
        return ResponseFactory.NoContent();
    }



    /// <summary>
    /// Delete account
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
    public async Task<IActionResult> DeleteAccount(string id)
    {
        await _accountService.DeleteAccountAsync(id);
        return ResponseFactory.NoContent();
    }
}
