using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using System.Data;

namespace Auth.Infrastructure.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

       
        public async Task<RoleReadDTO> CreateRoleAsync(RoleWriteDTO writeDTO)
        {
            await EnsureNameNotTakenByAnotherRole(writeDTO.Name);

            var role = _mapper.Map<Role>(writeDTO);

            await _unitOfWork.RoleRepository.AddAsync(role);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<RoleReadDTO>(role);
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _unitOfWork.RoleRepository.FindAsync(roleId);

            if (role == null) throw new EntityWithIDNotFoundException<Role>(roleId);

            //soft delete - updadate status
            role.IsDelete = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<RoleReadDTO> GetRoleAsync(string roleId)
        {
            var role = await _unitOfWork.RoleRepository.FindAsync(roleId);

            return _mapper.Map<RoleReadDTO>(role);
        }

        public async Task<IEnumerable<RoleReadDTO>> GetRolesAsync()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<RoleReadDTO>>(roles);
        }

        public  async Task<RoleReadDTO> UpdateRoleAsync(string roleId,RoleWriteDTO roleWriteDTO)
        {
            var role = await _unitOfWork.RoleRepository.FindAsync(roleId);
            
            if(role == null) throw new EntityWithIDNotFoundException<Role>(roleId);

            _mapper.Map(roleWriteDTO, role);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<RoleReadDTO>(role);

        }

        private async Task EnsureNameNotTakenByAnotherRole(string name, string? roleID = null)
        {
            var roleWithSameName = await _unitOfWork.RoleRepository.FindByNameIgnoreCaseAsync(name);

            if (roleWithSameName != null && (!roleID.IsNullOrEmpty() || roleWithSameName.Id != roleID))
            {
                throw new UniqueConstraintException<Role>(nameof(Role.Name), name);
            }
        }
    }
}
