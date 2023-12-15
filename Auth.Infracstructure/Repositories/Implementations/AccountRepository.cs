using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Core.Enum;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

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
            return await _context.Accounts.Include(acc=>acc.Role).FirstOrDefaultAsync(acc => acc.Username == inputDTO.Username && acc.Password == inputDTO.Password && !acc.IsDelete);
        }

        public async Task<IEnumerable<Account>> QueryAsync(AccountQuery query, bool trackChanges = false)
        {
            IQueryable<Account> accounts = _context.Accounts.Include(a => a.Role).Where(c=>c.IsDelete == false && c.RoleId != "1");

            if(!trackChanges)
            {
                accounts = accounts.AsNoTracking();
            }
            if (!string.IsNullOrEmpty(query.RoleId) && query.RoleId != ((int)RoleEnum.Admin).ToString())
            {
                accounts = accounts.Where(c => c.RoleId == query.RoleId);
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                accounts = accounts.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                accounts = accounts.Where(c=>c.Username.Contains(query.SearchText) || c.Name.Contains(query.SearchText));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                accounts = accounts.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Account> enumeratedAccounts = accounts.AsEnumerable();

            return await Task.FromResult(enumeratedAccounts);
        }
    }
}
