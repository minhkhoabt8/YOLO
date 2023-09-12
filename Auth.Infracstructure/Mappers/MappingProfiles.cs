using Auth.Core.Entities;
using Auth.Infracstructure.DTOs.Account;
using Auth.Infracstructure.DTOs.Role;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infracstructure.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Account, AccountReadDTO>();
            CreateMap<Role, RoleReadDTO>();
        }
    }
}
