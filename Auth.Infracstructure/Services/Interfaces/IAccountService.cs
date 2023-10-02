using Auth.Infrastructure.DTOs.Account;


namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync();
        Task<AccountReadDTO> CreateAccountAsync(AccountWriteDTO writeDTO);

        Task<AccountReadDTO> UpdateAccountAsync(string Id, AccountWriteDTO accountReadDTO);

        Task DeleteAccountAsync(string id);

        Task<AccountReadDTO> GetAccountByIdAsync(string id);

    }
}
