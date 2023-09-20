using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;

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
