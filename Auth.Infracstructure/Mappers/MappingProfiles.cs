using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Notification;
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
            //Notification
            CreateMap<Notification,NotificationReadDTO>();
            CreateMap<NotificationWriteDTO, Notification>();
        }
    }
}
