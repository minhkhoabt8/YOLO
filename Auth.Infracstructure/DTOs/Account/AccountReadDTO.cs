using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.DTOs.Account
{
    public class AccountReadDTO
    {
        public string Id { get; set; }

        public string Username { get; set; } 

        //public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Otp { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public RoleReadDTO Role { get; set; }

    }

}
