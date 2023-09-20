using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.DTOs.Role
{
    public class RoleReadDTO
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool? IsDelete { get; set; }
    }
}
