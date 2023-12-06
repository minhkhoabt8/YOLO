using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.DTOs.Authentication
{
    public class ResetPasswordInputDTO
    {
        [Required] public string Username { get; set; }
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Password cũ không khớp với password mới.")]
        public string ConfirmPassword { get; set; }
        
    }
}
