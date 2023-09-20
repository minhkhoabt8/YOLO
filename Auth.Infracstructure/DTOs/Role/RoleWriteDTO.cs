using System.ComponentModel.DataAnnotations;

namespace Auth.Infrastructure.DTOs.Role;

public class RoleWriteDTO
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
}