using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class LandGroupService : ILandGroupService
    {   
        private IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public LandGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(LandGroupWriteDTO delete)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindAsync(delete);
            if (landGroup == null)
            {
                throw new Exception("This landgroup not found");
            }
             _unitOfWork.LandGroupRepository.Delete(landGroup);
            await _unitOfWork.CommitAsync();
            return true;

        }

        public async Task<IEnumerable<LandGroupReadDTO>> GetAllLandGroupAsync()
        {
           var landGroups = _unitOfWork.LandGroupRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LandGroupReadDTO>>(landGroups);
        }

        public async Task<LandGroupReadDTO?> GetAsync(string code)
        {
            var landGroups = await _unitOfWork.LandGroupRepository.FindAsync(code);
            return _mapper.Map<LandGroupReadDTO>(landGroups);
        }

        public async Task<LandGroupReadDTO?> UpdateAsync(string id, LandGroupWriteDTO landGroupUpdateDTO)
        {
           var existLandgroup = await _unitOfWork.LandGroupRepository.FindAsync(id);
            if (existLandgroup == null)
            {
                throw new Exception("This landgroup not found");
            }
           
            existLandgroup.Name = landGroupUpdateDTO.Name;
            
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandGroupReadDTO>(existLandgroup);


        }

        public async Task<LandGroupReadDTO?> CreateLandgroupAsync(LandGroupWriteDTO landGroupWriteDTO)
        {   
            await EnsureLandGroupCodeNotDupicate(landGroupWriteDTO.Code);
            var landGroup = _mapper.Map<LandGroup>(landGroupWriteDTO);
            await _unitOfWork.LandGroupRepository.AddAsync(landGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandGroupReadDTO>(landGroup);
        }
        private async Task EnsureLandGroupCodeNotDupicate(string code)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAsync(code);
            if (landGroup != null)
            {
                throw new Exception("This landgroup code is already exist");
            }
        }

      
    }
}
