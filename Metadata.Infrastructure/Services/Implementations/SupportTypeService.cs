using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.SupportType;
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

        public async Task<SupportTypeReadDTO?> CreateLandTypeAsync(SupportTypeWriteDTO supportTypeWriteDTO)
        {
            EnsureSupportTypeCodeNotDupicate(supportTypeWriteDTO.Code);
            var supportType = _mapper.Map<SupportType>(supportTypeWriteDTO);
            await _unitOfWork.SupportTypeRepository.AddAsync(supportType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<SupportTypeReadDTO>(supportType);
        }

        public async Task<SupportTypeReadDTO?> UpdateAsync(string id, SupportTypeWriteDTO supportTypeUpdateDTO)
        {
           var supportType = await _unitOfWork.SupportTypeRepository.FindAsync(id);
            if (supportType == null)
            {
                throw new EntityWithIDNotFoundException<SupportType>(id);
            }
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

        private async Task EnsureSupportTypeCodeNotDupicate(string code)
        {
            var supportType = await _unitOfWork.SupportTypeRepository.FindByCodeAsync(code);
            if (supportType != null && supportType.Code == code)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(supportType.Code), code);
            }
        }
    }
}
