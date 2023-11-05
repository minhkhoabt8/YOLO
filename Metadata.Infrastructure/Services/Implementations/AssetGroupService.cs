using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
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
    public class AssetGroupService : IAssetGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssetGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssetGroupReadDTO> CreateAssetGroupAsync(AssetGroupWriteDTO assetGroupWriteDTO)
        {
            await EnsureAssetGroupCodeNotDuplicate(assetGroupWriteDTO.Code);
           
            var assetGroup = _mapper.Map<AssetGroup>(assetGroupWriteDTO);

            await _unitOfWork.AssetGroupRepository.AddAsync(assetGroup);
            
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetGroupReadDTO>(assetGroup);

        }
        
        public async Task<bool> DeleteAssetGroupAsync(string id)
        {
            var assetGroup = await _unitOfWork.AssetGroupRepository.FindAsync(id);
            if (assetGroup == null)
            {
                throw new EntityWithIDNotFoundException<AssetGroup>(id);
            }
            assetGroup.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<AssetGroupReadDTO>> GetAllAssetGroupsAsync()
        {
            var assetGroups = await _unitOfWork.AssetGroupRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AssetGroupReadDTO>>(assetGroups);
        }

        public async Task<IEnumerable<AssetGroupReadDTO>> GetAllDeletedAssetGroupAsync()
        {
            var assetGroups = await _unitOfWork.AssetGroupRepository.GetAllDeletedAssetGroup();
            return _mapper.Map<IEnumerable<AssetGroupReadDTO>>(assetGroups);
        }


        public async Task<AssetGroupReadDTO> GetAssetGroupAsync(string code)
        {   
            var landgroups = await _unitOfWork.AssetGroupRepository.FindAsync(code);
            return _mapper.Map<AssetGroupReadDTO>(landgroups);
        }

        public async Task<AssetGroupReadDTO> UpdateAssetGroupAsync(string id, AssetGroupWriteDTO assetGroupWriteDTO)
        {
            var existAssetGroup = await _unitOfWork.AssetGroupRepository.FindAsync(id);
            if (existAssetGroup == null)
            {
                throw new EntityWithIDNotFoundException<AssetGroup>(id);
            }
            /*await EnsureAssetGroupCodeNotDuplicateForUpdate(assetGroupWriteDTO.Code , assetGroupWriteDTO.Name , id);*/
              _mapper.Map(assetGroupWriteDTO, existAssetGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetGroupReadDTO>(existAssetGroup);
        }
        private async Task EnsureAssetGroupCodeNotDuplicate(string code)
        {
            var assetGroup = await _unitOfWork.AssetGroupRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (assetGroup != null && assetGroup.Code == code)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroup.Code), code);
            }
            var assetGroupByName = await _unitOfWork.AssetGroupRepository.FindByNameAndIsDeletedStatus(name, false);
            if (assetGroupByName != null && assetGroupByName.Name == name)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroupByName.Name), name);
            }
        }

        /*private async Task EnsureAssetGroupCodeNotDuplicateForUpdate(string code, string name , string id)
        {
            var assetGroup = await _unitOfWork.AssetGroupRepository.FindByCodeAndIsDeletedStatusForUpdate(code,id , false);
            if (assetGroup != null && assetGroup.Code == code && assetGroup.AssetGroupId != id)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroup.Code), code);
            }
            var assetGroupByName = await _unitOfWork.AssetGroupRepository.FindByCodeAndIsDeletedStatusForUpdate(name,id, false);
            if (assetGroupByName != null && assetGroupByName.Name == name && assetGroupByName.AssetGroupId != id)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroupByName.Name), name);
            }
        }*/

        public async Task CheckNameAssetGroupNotDuplicate (string name)
        {
            var assetGroup = await _unitOfWork.AssetGroupRepository.FindByNameAndIsDeletedStatus(name, false);
            if(assetGroup!= null && assetGroup.Name == name)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroup.Name), name);
            }
           
        }

        public async Task CheckCodeAssetGroupNotDuplicate (string code)
        {
            var assetGroup = await _unitOfWork.AssetGroupRepository.FindByCodeAndIsDeletedStatus(code, false);
            if(assetGroup!= null && assetGroup.Code == code)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroup.Code), code);
            }
        }

        
    }
}
