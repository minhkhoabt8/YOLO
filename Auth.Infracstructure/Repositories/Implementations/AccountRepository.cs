using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Repositories.Interfaces;
using Metadata.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
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
            IQueryable<Account> acounts = _context.Accounts.Include(a => a.Role);

            if(!trackChanges)
            {
                acounts = acounts.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(query.Include))
            {
                acounts = acounts.IncludeDynamic(query.Include);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                acounts = acounts.FilterAndOrderByTextSimilarity(query.SearchText, 50);
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                acounts = acounts.OrderByDynamic(query.OrderBy);
            }
            IEnumerable<Account> enumeratedAccounts = acounts.AsEnumerable();

            return await Task.FromResult(enumeratedAccounts);
        }
    }
}
