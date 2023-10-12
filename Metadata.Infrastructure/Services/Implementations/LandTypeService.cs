
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandType;
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
    public class LandTypeService : ILandTypeService
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LandTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LandTypeReadDTO?> CreateLandTypeAsync(LandTypeWriteDTO landTypeWriteDTO)
        {
            await EnsureLandGroupCodeNotDupicate(landTypeWriteDTO.Code);
            await CheckDeleteStatus(landTypeWriteDTO.Code);
            var landType = _mapper.Map<LandType>(landTypeWriteDTO);
            await _unitOfWork.LandTypeRepository.AddAsync(landType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandTypeReadDTO>(landType);
        }

        public async Task<bool> DeleteAsync(LandTypeWriteDTO delete)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindAsync(delete);   
            if (landType == null)
            {
                throw new EntityWithIDNotFoundException<LandType>(delete);
            }
            landType.IsDeleted = true;

            _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<LandTypeReadDTO>> GetAllLandTypeAsync()
        {
            var landTypes = await _unitOfWork.LandTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LandTypeReadDTO>>(landTypes);
        }

        public async Task<LandTypeReadDTO?> GetAsync(string code)
        {
            var landTypes = await _unitOfWork.LandTypeRepository.FindAsync(code);
            return _mapper.Map<LandTypeReadDTO>(landTypes);
        }

        public async Task<LandTypeReadDTO?> UpdateAsync(string id, LandTypeWriteDTO landTypeUpdateDTO)
        {
            var existLandType = await _unitOfWork.LandTypeRepository.FindAsync(id);
            if (existLandType == null)
            {
                throw new EntityWithIDNotFoundException<LandType>(id);
            }
            _mapper.Map(landTypeUpdateDTO, existLandType);
             await _unitOfWork.CommitAsync();
            return _mapper.Map<LandTypeReadDTO>(existLandType);
        }

        private async Task EnsureLandGroupCodeNotDupicate(string code)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindByCodeAsync(code);
            if (landType != null && landType.Code == code)
            {
                throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType.Code), code);
            }
        }
        private async Task CheckDeleteStatus(string id)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindAsync(id);
            if (landType != null && landType.IsDeleted == true)
            {
                throw new Exception("This landgroup code is already delete");
            }

        }

    }
}
