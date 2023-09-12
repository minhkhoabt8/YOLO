using Auth.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.Repositories.Interfaces
{
    public interface IAccountRepository :
        IAddAsync<Account>,
        IGetAllAsync<Account>,
        IFindAsync<Account>
    {
    }
}
