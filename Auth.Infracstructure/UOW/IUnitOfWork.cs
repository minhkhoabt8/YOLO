using Auth.Infracstructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.UOW
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
    }
}
