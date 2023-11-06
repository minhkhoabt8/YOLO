using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.SupportType;
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
    public class SupportTypeService : ISupportTypeService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupportTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupportTypeReadDTO>> GetAllLandTypeAsync()
        {
            return _mapper.Map<IEnumerable<SupportTypeReadDTO>>(await _unitOfWork.SupportTypeRepository.GetAllAsync());
        }

        public async Task<SupportTypeReadDTO?> GetAsync(string code)
        {
            var supportType = await _unitOfWork.SupportTypeRepository.FindAsync(code);
            return _mapper.Map<SupportTypeReadDTO>(supportType);
        }

        public async Task<IEnumerable<SupportTypeReadDTO>> GetAllActivedLandTypeAsync()
        {
            return _mapper.Map<IEnumerable<SupportTypeReadDTO>>(await _unitOfWork.SupportTypeRepository.GetAllActivedSupportType());
        }

        public async Task<SupportTypeReadDTO?> CreateLandTypeAsync(SupportTypeWriteDTO supportTypeWriteDTO)
        {
            await EnsureSupportTypeCodeNotDuplicate(supportTypeWriteDTO.Code, supportTypeWriteDTO.Name);
            var supportType = _mapper.Map<SupportType>(supportTypeWriteDTO);
            await _unitOfWork.SupportTypeRepository.AddAsync(supportType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<SupportTypeReadDTO>(supportType);
        }

        public async Task<IEnumerable<SupportTypeReadDTO>> CreateLandTypesAsync(IEnumerable<SupportTypeWriteDTO> supportTypeWriteDTOs)
        {
            var supportTypes = new List<SupportTypeReadDTO>();
            foreach (var supportTypeDTO in supportTypeWriteDTOs)
            {
                await EnsureSupportTypeCodeNotDuplicate(supportTypeDTO.Code, supportTypeDTO.Name);
                var supportType = _mapper.Map<SupportType>(supportTypeDTO);
                await _unitOfWork.SupportTypeRepository.AddAsync(supportType);
                await _unitOfWork.CommitAsync();
                var readDTO = _mapper.Map<SupportTypeReadDTO>(supportType);
                supportTypes.Add(readDTO);
            }
            return supportTypes;
        }

        public async Task<SupportTypeReadDTO?> UpdateAsync(string id, SupportTypeWriteDTO supportTypeUpdateDTO)
        {
           var supportType = await _unitOfWork.SupportTypeRepository.FindAsync(id);
            if (supportType == null)
            {
                throw new EntityWithIDNotFoundException<SupportType>(id);
            }   
            await EnsureSupportTypeCodeNotDuplicateForUpdate(supportTypeUpdateDTO.Code, supportTypeUpdateDTO.Name,id);
             _mapper.Map(supportTypeUpdateDTO, supportType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<SupportTypeReadDTO>(supportType);
            
        }

        public async Task<bool> DeleteAsync(string id)
        {   
            var supportType = await _unitOfWork.SupportTypeRepository.FindAsync(id);
            if(supportType == null)
            {
                throw new EntityWithIDNotFoundException<SupportType>(id);
            }
            supportType.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;
        }

        private async Task EnsureSupportTypeCodeNotDuplicate(string code , string name)
        {
            var supportType = await _unitOfWork.SupportTypeRepository.FindByCodeAndIsDeletedStatus(code,false);
            if (supportType != null && supportType.Code == code)
            {
                throw new UniqueConstraintException<SupportType>(nameof(supportType.Code), code);
            }
            var supportType1 = await _unitOfWork.SupportTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (supportType1 != null && supportType1.Name == name)
            {
                throw new UniqueConstraintException<SupportType>(nameof(supportType1.Name), name);
            }
        }

        private async Task EnsureSupportTypeCodeNotDuplicateForUpdate(string code, string name, string id)
        {
            var supportType = await _unitOfWork.SupportTypeRepository.FindByCodeAndIsDeletedStatusForUpdate(code, id, false);
            if (supportType != null && supportType.Code == code && supportType.SupportTypeId != id)
            {
                throw new UniqueConstraintException<SupportType>(nameof(supportType.Code), code);
            }
            var supportType2 = await _unitOfWork.SupportTypeRepository.FindByNameAndIsDeletedStatusForUpdate(name, id, false);
            if (supportType2 != null && supportType2.Name == name && supportType2.SupportTypeId != id)
            {
                throw new UniqueConstraintException<SupportType>(nameof(supportType2.Name), name);
            }
        }

        public async Task CheckNameSupportTypeNotDuplicate(string name)
        {
            var supportType = await _unitOfWork.SupportTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (supportType != null && supportType.Name == name)
            {
                throw new UniqueConstraintException<SupportType>(nameof(supportType.Name), name);
            }
        }

        public async Task CheckCodeSupportTypeNotDuplicate(string code)
        {
            var supportType = await _unitOfWork.SupportTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (supportType != null && supportType.Code == code)
            {
                throw new UniqueConstraintException<SupportType>(nameof(supportType.Code), code);
            }
        }

        public async Task<PaginatedResponse<SupportTypeReadDTO>> QuerySupportTypeAsync(SupportTypeQuery paginationQuery)
        {
            var supportTypes = await _unitOfWork.SupportTypeRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<SupportTypeReadDTO>.FromEnumerableWithMapping(supportTypes, paginationQuery, _mapper);
        }
    }
}
