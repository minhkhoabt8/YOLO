using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;


namespace Auth.Infrastructure.Repositories.Implementations
{
    public class RoleRepository : GenericRepository<Role, YoloAuthContext>, IRoleRepository
    {
        public RoleRepository(YoloAuthContext context) : base(context)
        {
        }
        
        public Task<Role?> FindByNameIgnoreCaseAsync(string roleName)
        {
            return _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
        }

        public async Task<Role> GetAccountRolesAsync(string accountID)
        {
            var account = await _context.Accounts.Include(acc => acc.Role).FirstAsync(acc => acc.Id == accountID);

            return account.Role;
        }
    }
}
