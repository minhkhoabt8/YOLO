using Auth.Infrastructure.DTOs.Account;


namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync();
       
    }
}
