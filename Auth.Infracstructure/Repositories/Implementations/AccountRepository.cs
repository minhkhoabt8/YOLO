using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;


namespace Auth.Infrastructure.Repositories.Implementations
{
    public class AccountRepository : GenericRepository<Account, YoloAuthContext>, IAccountRepository
    {
        public AccountRepository(YoloAuthContext context) : base(context)
        {
        }

        public async Task<Account?> FindAccountByUsernameAsync(string username)
        {
            return await _context.Accounts.Include(acc => acc.Role).FirstOrDefaultAsync(acc => acc.Username == username);
        }

        public async Task<Account?> LoginAsync(LoginInputDTO inputDTO)
        {
            return await _context.Accounts.Include(acc=>acc.Role).FirstOrDefaultAsync(acc => acc.Username == inputDTO.Username && acc.Password == inputDTO.Password);
        }
    }
}
