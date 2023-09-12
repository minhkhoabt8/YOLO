using Auth.Infracstructure.DTOs.Account;
using Auth.Infracstructure.Repositories.Interfaces;
using Auth.Infracstructure.Services.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync(include: "Role");

            return _mapper.Map<IEnumerable<AccountReadDTO>>(accounts);
        }
    }
}
