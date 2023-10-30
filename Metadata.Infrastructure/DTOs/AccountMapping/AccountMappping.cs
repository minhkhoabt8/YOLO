using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AccountMapping
{
    public class AccountMappping
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Otp { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public RoleReadDTO Role { get; set; }
    }

    public class RoleReadDTO
    {
        public string Id { get; set; }

        public string Name { get; set; } 

        public bool? IsDelete { get; set; }
    }
}
