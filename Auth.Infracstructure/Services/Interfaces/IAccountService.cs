using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using SharedLib.Infrastructure.DTOs;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync();

        Task<AccountReadDTO> CreateAccountAsync(AccountWriteDTO writeDTO);

        Task<AccountReadDTO> UpdateAccountAsync(string Id, AccountUpdateDTO accountUpdateDTO);

        Task DeleteAccountAsync(string id);

        Task<AccountReadDTO> GetAccountByIdAsync(string id);

        Task<PaginatedResponse<AccountReadDTO>> QueryAccount(AccountQuery query);

        Task UpdatePasswordAsync(ResetPasswordInputDTO dto);
    }
}
