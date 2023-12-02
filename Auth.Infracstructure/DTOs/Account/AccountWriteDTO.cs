using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Auth.Core.Entities;
using Auth.Core.Enum;

namespace Auth.Infrastructure.DTOs.Account
{
    public class AccountWriteDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Username { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 9)]
        public string Phone { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
