using Auth.Infracstructure.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync();
    }
}
