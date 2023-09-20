using Auth.Core.Data;
using Auth.Core.Entities;
using SharedLib.Infrastructure.Repositories.Implementations;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository:
        IGetAllAsync<Role>,
        IFindAsync<Role>,
        IAddAsync<Role>,
        IDelete<Role>
    {
        Task<Role?> FindByNameIgnoreCaseAsync(string roleName);
        Task<Role> GetAccountRolesAsync(string accountID);
    }
}
