using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using SharedLib.Infrastructure.Repositories.Interfaces;

namespace Auth.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository :
        IAddAsync<Account>,
        IGetAllAsync<Account>,
        IFindAsync<Account>,
        IQueryAsync<Account, AccountQuery>
    {
        Task<Account?> LoginAsync(LoginInputDTO inputDTO);

        Task<Account?> FindAccountByUsernameAsync(string username);
        Task<Account?> FindAccountByPhoneAsync(string phone);
        Task<Account?> FindAccountByEmailAsync(string email);
    }
}
