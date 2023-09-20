
using Auth.Infrastructure.DTOs.Role;


namespace Auth.Infrastructure.Services.Interfaces
{
    public  interface IRoleService
    {
        Task<RoleReadDTO> CreateRoleAsync(RoleWriteDTO writeDTO);
    }
}
