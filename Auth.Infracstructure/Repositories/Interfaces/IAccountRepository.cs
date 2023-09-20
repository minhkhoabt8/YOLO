using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Authentication;
using SharedLib.Infrastructure.Repositories.Interfaces;

namespace Auth.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository :
        IAddAsync<Account>,
        IGetAllAsync<Account>,
        IFindAsync<Account>
    {
        Task<Account?> LoginAsync(LoginInputDTO inputDTO);
    }
}
