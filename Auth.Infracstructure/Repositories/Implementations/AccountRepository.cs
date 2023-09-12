using Auth.Core.Data;
using Auth.Core.Entities;
using Auth.Infracstructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.Repositories.Implementations
{
    public class AccountRepository : GenericRepository<Account, YoloAuthContext>, IAccountRepository
    {
        public AccountRepository(YoloAuthContext context) : base(context)
        {
        }
    }
}
