using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
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

        public async Task<IEnumerable<OrganizationTypeReadDTO>> GetAllActivedOrganizationTypeAsync()
        {
            var organizationTypes = await _unitOfWork.OrganizationTypeRepository.GetAllActivedOrganizationTypes();
            return _mapper.Map<IEnumerable<OrganizationTypeReadDTO>>(organizationTypes);
        }

        public async Task<OrganizationTypeReadDTO?> GetAsync(string code)
        {
            var organizationTypes = await _unitOfWork.OrganizationTypeRepository.FindAsync(code);
            return _mapper.Map<OrganizationTypeReadDTO>(organizationTypes);
        }

        public async Task<OrganizationTypeReadDTO?> CreateOrganizationTypeAsync(OrganizationTypeWriteDTO organizationTypeWriteDTO)
        {
            await EnsureOrganizationTypeCodeNotDuplicate(organizationTypeWriteDTO.Code, organizationTypeWriteDTO.Name);
           
            var organizationType = _mapper.Map<OrganizationType>(organizationTypeWriteDTO);
            await _unitOfWork.OrganizationTypeRepository.AddAsync(organizationType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrganizationTypeReadDTO>(organizationType);

        }

        public async Task<IEnumerable<OrganizationTypeReadDTO>> CreateOrganizationTypesAsync(IEnumerable<OrganizationTypeWriteDTO> organizationTypeWriteDTOs)
        {
            var organizationTypes = new List<OrganizationTypeReadDTO>();

            foreach (var organizationTypeDTO in organizationTypeWriteDTOs)
            {
                await EnsureOrganizationTypeCodeNotDuplicate(organizationTypeDTO.Code, organizationTypeDTO.Name);

                var organizationType = _mapper.Map<OrganizationType>(organizationTypeDTO);

                await _unitOfWork.OrganizationTypeRepository.AddAsync(organizationType);
                await _unitOfWork.CommitAsync();

                var readDTO = _mapper.Map<OrganizationTypeReadDTO>(organizationType);

                organizationTypes.Add(readDTO);
            }

            return organizationTypes;
        }

        public async Task<OrganizationTypeReadDTO?> UpdateAsync(string id, OrganizationTypeWriteDTO organizationTypeUpdateDTO)
        {
            var existOrganizationType = await _unitOfWork.OrganizationTypeRepository.FindAsync(id);
            if (existOrganizationType == null)
            {
                throw new EntityWithIDNotFoundException<OrganizationType>(id);
            }
            await EnsureOrganizationTypeCodeNotDuplicateForUpdate(organizationTypeUpdateDTO.Code, organizationTypeUpdateDTO.Name,id);
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

        private async Task EnsureOrganizationTypeCodeNotDuplicate(string code , string name)
        {
            var organizationType = await _unitOfWork.OrganizationTypeRepository.FindByCodeAndIsDeletedStatus(code,true);
            if (organizationType != null && organizationType.Code == code)
            {
                throw new UniqueConstraintException<OrganizationType>(nameof(organizationType.Code), code);
            }
            var organizationType1 = await _unitOfWork.OrganizationTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (organizationType1 != null && organizationType1.Name == name)
            {
                throw new UniqueConstraintException<OrganizationType>(nameof(organizationType1.Name), name);
            }
        }
        //EnsureOrganizationTypeCodeNotDuplicateforUpdate
        private async Task EnsureOrganizationTypeCodeNotDuplicateForUpdate(string code, string name,string id)
        {
            var organizationType = await _unitOfWork.OrganizationTypeRepository.FindByCodeAndIsDeletedStatusForUpdate(code,id, false);
            if (organizationType != null && organizationType.Code == code && organizationType.OrganizationTypeId != id)
            {
                throw new UniqueConstraintException<OrganizationType>(nameof(organizationType.Code), code);
            }
            var organizationType1 = await _unitOfWork.OrganizationTypeRepository.FindByNameAndIsDeletedStatusForUpdate(name,id, false);
            if (organizationType1 != null && organizationType1.Name == name && organizationType.OrganizationTypeId != id)
            {
                throw new UniqueConstraintException<OrganizationType>(nameof(organizationType1.Name), name);
            }
        }

        public async Task CheckNameOrganizationTypeNotDuplicate(string name)
        {
            var organizationType = await _unitOfWork.OrganizationTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (organizationType != null && organizationType.Name == name)
            {
                throw new UniqueConstraintException<OrganizationType>(nameof(organizationType.Name), name);
            }
        }
       
        public async Task CheckCodeOrganizationTypeNotDuplicate(string code)
        {
            var organizationType = await _unitOfWork.OrganizationTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (organizationType != null && organizationType.Code == code)
            {
                throw new UniqueConstraintException<OrganizationType>(nameof(organizationType.Code), code);
            }
        }

        public async Task<PaginatedResponse<OrganizationTypeReadDTO>> QueryOrganizationTypeAsync(OrganizationTypeQuery paginationQuery)
        {
            var organizationTypes = await _unitOfWork.OrganizationTypeRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<OrganizationTypeReadDTO>.FromEnumerableWithMapping(organizationTypes, paginationQuery, _mapper);
        }
    }
}
