
using Auth.Infrastructure.DTOs.Role;


namespace Auth.Infrastructure.Services.Interfaces
{
    public  interface IRoleService
    {
        Task<RoleReadDTO> CreateRoleAsync(RoleWriteDTO writeDTO);

        Task<RoleReadDTO> UpdateRoleAsync(string roleId, RoleWriteDTO roleWriteDTO);

        Task DeleteRoleAsync(string roleId);

        Task<RoleReadDTO> GetRoleAsync(string roleId);

        Task<IEnumerable<RoleReadDTO>> GetRolesAsync();

    }
}
