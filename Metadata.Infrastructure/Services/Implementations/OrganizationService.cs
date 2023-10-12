using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class OrganizationService : IOrganizationService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrganizationTypeReadDTO>> GetAllOrganizationTypeAsync()
        {
            var organizationTypes = await _unitOfWork.OrganizationTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrganizationTypeReadDTO>>(organizationTypes);
        }

        public async Task<OrganizationTypeReadDTO?> GetAsync(string code)
        {
            var organizationTypes = await _unitOfWork.OrganizationTypeRepository.FindAsync(code);
            return _mapper.Map<OrganizationTypeReadDTO>(organizationTypes);
        }

        public async Task<OrganizationTypeReadDTO?> CreateOrganizationTypeAsync(OrganizationTypeWriteDTO organizationTypeWriteDTO)
        {
            await EnsureOrganizationTypeCodeNotDuplicate(organizationTypeWriteDTO.Code);
            await CheckDeleteStatus(organizationTypeWriteDTO.Code);
            var organizationType = _mapper.Map<OrganizationType>(organizationTypeWriteDTO);
            await _unitOfWork.OrganizationTypeRepository.AddAsync(organizationType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrganizationTypeReadDTO>(organizationType);

        }

        public async Task<OrganizationTypeReadDTO?> UpdateAsync(string id, OrganizationTypeWriteDTO organizationTypeUpdateDTO)
        {
            var existOrganizationType = await _unitOfWork.OrganizationTypeRepository.FindAsync(id);
            if (existOrganizationType == null)
            {
                throw new EntityWithIDNotFoundException<OrganizationType>(id);
            }
            _mapper.Map(organizationTypeUpdateDTO, existOrganizationType);
       
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrganizationTypeReadDTO>(existOrganizationType);
            
        }

        public async Task<bool> DeleteAsync(string delete)
        {
            var organizationType = await _unitOfWork.OrganizationTypeRepository.FindAsync(delete);
            if (organizationType == null)
            {
                throw new EntityWithIDNotFoundException<OrganizationType>(delete);
            }
            organizationType.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;
        }

        private async Task EnsureOrganizationTypeCodeNotDuplicate(string code)
        {
            var organizationType = await _unitOfWork.LandGroupRepository.FindByCodeAsync(code);
            if (organizationType != null && organizationType.Code == code)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(organizationType.Code), code);
            }
        }

        private async Task CheckDeleteStatus(string id)
        {
            var organizationType = await _unitOfWork.OrganizationTypeRepository.FindAsync(id);
            if (organizationType != null && organizationType.IsDeleted == true)
            {
                throw new Exception("This landgroup code is already delete");
            }

        }
    }
}
