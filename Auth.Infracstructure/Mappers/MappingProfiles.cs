using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Role;
using AutoMapper;

namespace Auth.Infrastructure.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Role
            CreateMap<Role, RoleReadDTO>();
            CreateMap<RoleWriteDTO, Role>();

            //Account
            CreateMap<Account, AccountReadDTO>();
            CreateMap<AccountWriteDTO, Account>();

        }
    }
}
