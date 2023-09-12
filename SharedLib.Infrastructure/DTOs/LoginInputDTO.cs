using System.ComponentModel.DataAnnotations;

namespace SharedLib.Infrastructure.DTOs;

public class LoginInputDTO
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}