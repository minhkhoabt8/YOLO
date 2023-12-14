using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using LoginInputDTO = Auth.Infrastructure.DTOs.Authentication.LoginInputDTO;

namespace Auth.Infrastructure.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;
        private readonly IPasswordService _passwordService;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ISmsService smsService, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _smsService = smsService;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync(include: "Role");
            return _mapper.Map<IEnumerable<AccountReadDTO>>(accounts);
        }
        
        public async Task<AccountReadDTO> CreateAccountAsync(AccountWriteDTO writeDTO)
        {

            //check account username exist
            var existAccount = await _unitOfWork.AccountRepository.FindAccountByUsernameAsync(writeDTO.Username);

            if (existAccount != null)
            {
                throw new UniqueConstraintException<Account>(nameof(Account.Username), writeDTO.Username);
            }

            var newAccount = _mapper.Map<Account>(writeDTO);

            if (writeDTO.FullName.IsNullOrEmpty())
            {
                newAccount.Name = writeDTO.Username;
            }

            newAccount.Name = writeDTO.FullName;
            
            //Generate HasedPassword based on random Password
            var rawPassword = GeneratePassword();

            newAccount.Password = _passwordService.GenerateHashPassword(rawPassword);

            await _unitOfWork.AccountRepository.AddAsync(newAccount);

            //call Send SMS
            await _smsService.SendPasswordSmsAsync(newAccount.Phone!, rawPassword);

            await _smsService.SendPasswordEmail(newAccount.Email!, rawPassword);

            await _unitOfWork.AccountRepository.AddAsync(newAccount);

            await _unitOfWork.CommitAsync();

            newAccount.Role = await _unitOfWork.RoleRepository.FindAsync(writeDTO.RoleId);

            return _mapper.Map<AccountReadDTO>(newAccount);
        }

        
        private string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            char[] password = new char[12];

            Random random = new Random();

            for (int i = 0; i < 12; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }

        

        public async Task<AccountReadDTO> UpdateAccountAsync(string Id, AccountUpdateDTO accountUpdateDTO)
        {
            var account = await _unitOfWork.AccountRepository.FindAsync(Id);

            if (account == null) throw new EntityWithIDNotFoundException<Account>(Id);

            if (!account.Name.IsNullOrEmpty())
            {
                account.Name = accountUpdateDTO.FullName!;
            }

            if (!account.RoleId.IsNullOrEmpty())
            {
                account.RoleId = accountUpdateDTO.RoleId!;
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AccountReadDTO>(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            var account = await _unitOfWork.AccountRepository.FindAsync(id);
            if (account == null) throw new EntityWithIDNotFoundException<Account>(id);
            account.IsDelete = true;
            await _unitOfWork.CommitAsync();
        }

        public async Task<AccountReadDTO> GetAccountByIdAsync(string id)
        {
            return _mapper.Map<AccountReadDTO>(await _unitOfWork.AccountRepository.FindAsync(id, include: "Role"));
        }

        public async Task<PaginatedResponse<AccountReadDTO>> QueryAccount(AccountQuery query)
        {
            var accounts = await _unitOfWork.AccountRepository.QueryAsync(query);

            return PaginatedResponse<AccountReadDTO>.FromEnumerableWithMapping(accounts, query, _mapper);
        }

        public async Task UpdatePasswordAsync(ResetPasswordInputDTO dto)
        {
            //1.try log in using old password
            //var login = new LoginInputDTO
            //{
            //    Username = dto.Username,
            //    Password = dto.OldPassword
            //};

            //2.check account exist with username and old password
            var account = await _unitOfWork.AccountRepository.FindAccountByUsernameAsync(dto.Username);

            if (account == null || account.IsDelete)
            {
                throw new WrongCredentialsException();
            }

            //2.1.Verify user input password and system hashed password
            var hashedPasswordWithSalt = account.Password.ToString().Split("-");

            var isValidPassword = _passwordService.ValidatePassword(dto.OldPassword, hashedPasswordWithSalt[0], hashedPasswordWithSalt[1]);

            if (!isValidPassword)
            {
                throw new WrongCredentialsException();
            }

            //2.2.Update Password
            account.Password = _passwordService.GenerateHashPassword(dto.NewPassword);


            await _unitOfWork.CommitAsync();
        }
    }
}
