using Auth.Infracstructure.DTOs.Account;
using Auth.Infracstructure.Repositories.Interfaces;
using Auth.Infracstructure.Services.Interfaces;
using Auth.Infracstructure.UOW;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync(include: "Role");

            return _mapper.Map<IEnumerable<AccountReadDTO>>(accounts);
        }
    }
}
