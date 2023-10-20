using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.LandGroup;
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
    public class DeductionTypeService : IDeductionTypeService
    {   
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeductionTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DeductionTypeReadDTO> AddDeductionType(DeductionTypeWriteDTO deductionType)
        {
            await EnsureDeductionTypeNotExist(deductionType.Code);
            var newDeductionType = _mapper.Map<DeductionType>(deductionType);
            await _unitOfWork.DeductionTypeRepository.AddAsync(newDeductionType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DeductionTypeReadDTO>(newDeductionType);
        }



        public async Task<bool> DeleteDeductionTypeAsync(string id)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindAsync(id);
            if (existDeductionType == null)
            {
                throw new EntityWithIDNotFoundException<DeductionType>(id);
            }
            existDeductionType.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<DeductionTypeReadDTO>> GetAllDeductionTypesAsync()
        {
            var deductionTypes = await _unitOfWork.DeductionTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DeductionTypeReadDTO>>(deductionTypes);
        }

        public async Task<IEnumerable<DeductionTypeReadDTO>> GetAllDeletedDeductionTypesAsync()
        {
            var deductionTypes = await _unitOfWork.DeductionTypeRepository.GetAllDeletedDeductionTypes();
            return _mapper.Map<IEnumerable<DeductionTypeReadDTO>>(deductionTypes);
        }

        public async Task<DeductionTypeReadDTO> GetDeductionTypeAsync(string id)
        {
            var deductionType = await _unitOfWork.DeductionTypeRepository.FindAsync(id);
            return _mapper.Map<DeductionTypeReadDTO>(deductionType);
        }

        public async Task<DeductionTypeReadDTO> UpdateDeductionTypeAsync(string id, DeductionTypeWriteDTO deductionType)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindAsync(id);
            if (existDeductionType == null)
            {
                throw new EntityWithIDNotFoundException<DeductionType>(id);
            }
            _mapper.Map(deductionType, existDeductionType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DeductionTypeReadDTO>(existDeductionType);
        }

        private async Task EnsureDeductionTypeNotExist(string code)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindByCodeAndIsDeletedStatus(code, true);
            if (existDeductionType != null && existDeductionType.Code==code)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType.Code),code);
            }
        }
    }
}
