using AutoMapper;
using Metadata.Core.Entities;
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
    public class LandGroupService : ILandGroupService
    {   
        private IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public LandGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(string delete)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindAsync(delete);
            if (landGroup == null)
            {
                throw new EntityWithIDNotFoundException<LandGroup>(delete);
            }
             landGroup.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;

        }

        public async Task<IEnumerable<LandGroupReadDTO>> GetAllLandGroupAsync()
        {
           var landGroups = await _unitOfWork.LandGroupRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LandGroupReadDTO>>(landGroups);
        }

        public async Task<IEnumerable<LandGroupReadDTO>> GetAllDeletedLandGroupAsync()
        {
            var landGroups = await _unitOfWork.LandGroupRepository.GetAllDeletedLandGroups();
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
                throw new EntityWithIDNotFoundException<LandGroup>(id);
            }
           
            _mapper.Map(landGroupUpdateDTO, existLandgroup);
            
            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandGroupReadDTO>(existLandgroup);

        }

        public async Task<LandGroupReadDTO?> CreateLandgroupAsync(LandGroupWriteDTO landGroupWriteDTO)
        {
            await EnsureLandGroupCodeNotDuplicate(landGroupWriteDTO.Code);
            
            var landGroup = _mapper.Map<LandGroup>(landGroupWriteDTO);
            await _unitOfWork.LandGroupRepository.AddAsync(landGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandGroupReadDTO>(landGroup);
        }
        public async Task<IEnumerable<LandGroupReadDTO>> CreateListLandGroupAsync(IEnumerable<LandGroupWriteDTO> landGroupWriteDTOs)
        { 
            var landGroups = new List<LandGroupReadDTO>();
            foreach (var landGroupWriteDTO in landGroupWriteDTOs)
            {
                await EnsureLandGroupCodeNotDuplicate(landGroupWriteDTO.Code, landGroupWriteDTO.Name);
                var landGroup = _mapper.Map<LandGroup>(landGroupWriteDTO);
                await _unitOfWork.LandGroupRepository.AddAsync(landGroup);
                await _unitOfWork.CommitAsync();

                var landGroupReadDTO = _mapper.Map<LandGroupReadDTO>(landGroup);
                landGroups.Add(landGroupReadDTO);
            }
            return landGroups;
        }

        private async Task EnsureLandGroupCodeNotDuplicate(string code,string name)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAndIsDeletedStatus(code,false);
            if (landGroup != null && landGroup.Code == code )
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Code), code);
            }
            
            var landGroup2 = await _unitOfWork.LandGroupRepository.FindByNameAndIsDeletedStatus(name, false);
            if (landGroup2 != null  && landGroup2.Name == name)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup2.Name), name);
            }
        }
        public async Task<LandGroupReadDTO?> UpdateAsync(string id, LandGroupWriteDTO landGroupUpdateDTO)
        {
            var existLandgroup = await _unitOfWork.LandGroupRepository.FindAsync(id);
            if (existLandgroup == null)
            {
                throw new EntityWithIDNotFoundException<LandGroup>(id);
            }
            await EnsureLandGroupCodeNotDuplicate(landGroupUpdateDTO.Code, landGroupUpdateDTO.Name);
            _mapper.Map(landGroupUpdateDTO, existLandgroup);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandGroupReadDTO>(existLandgroup);


        }
        
        public async Task CheckNameLandGroupNotDuplicate(string name)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByNameAndIsDeletedStatus(name, false);
            if (landGroup != null && landGroup.Name == name)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Name), name);
            }
        }
        public async Task CheckCodeLandGroupNotDuplicate(string code)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (landGroup != null && landGroup.Code == code)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Code), code);
            }
        }
        
      
    }
}
