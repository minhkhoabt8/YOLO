using Auth.Infrastructure.DTOs.Account;
using SharedLib.Infrastructure.DTOs;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync();

        Task<AccountReadDTO> CreateAccountAsync(AccountWriteDTO writeDTO);

        Task<AccountReadDTO> UpdateAccountAsync(string Id, AccountWriteDTO accountReadDTO);

        Task DeleteAccountAsync(string id);

        Task<AccountReadDTO> GetAccountByIdAsync(string id);

        Task<PaginatedResponse<AccountReadDTO>> QueryAccount(AccountQuery query);
    }
}
