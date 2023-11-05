using Amazon.Runtime;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AuditTrail;
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
            await EnsureAssetGroupCodeNotDuplicate(assetGroupWriteDTO.Code , assetGroupWriteDTO.Name);
           
            var assetGroup = _mapper.Map<AssetGroup>(assetGroupWriteDTO);

            await _unitOfWork.AssetGroupRepository.AddAsync(assetGroup);
            
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetGroupReadDTO>(assetGroup);

        }
        public async Task<IEnumerable<AssetGroupReadDTO>> CreateAssetGroupsAsync(IEnumerable<AssetGroupWriteDTO> assetGroupWriteDTOs)
        {
            var assetGroups = new List<AssetGroupReadDTO>();

            foreach (var assetGroupDTO in assetGroupWriteDTOs)
            {
                await EnsureAssetGroupCodeNotDuplicate(assetGroupDTO.Code , assetGroupDTO.Name);

                var assetGroup = _mapper.Map<AssetGroup>(assetGroupDTO);

                await _unitOfWork.AssetGroupRepository.AddAsync(assetGroup);
                await _unitOfWork.CommitAsync();

                var readDTO = _mapper.Map<AssetGroupReadDTO>(assetGroup);

                assetGroups.Add(readDTO);
            }

            return assetGroups;
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

        public async Task<IEnumerable<AssetGroupReadDTO>> GetAllActivedAssetGroupAsync()
        {
            var assetGroups = await _unitOfWork.AssetGroupRepository.GetAllActivedDeletedAssetGroup();
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
            await EnsureAssetGroupCodeNotDuplicate(assetGroupWriteDTO.Code , assetGroupWriteDTO.Name);
              _mapper.Map(assetGroupWriteDTO, existAssetGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetGroupReadDTO>(existAssetGroup);
        }
        private async Task EnsureAssetGroupCodeNotDuplicate(string code , string name)
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

        public async Task<SharedLib.Infrastructure.DTOs.PaginatedResponse<AssetGroupReadDTO>> QueryAssetGroupAsync(AssetGroupQuery paginationQuery)
        {
            var assetGroups = await _unitOfWork.AssetGroupRepository.QueryAsync(paginationQuery);

            return SharedLib.Infrastructure.DTOs.PaginatedResponse<AssetGroupReadDTO>.FromEnumerableWithMapping(assetGroups, paginationQuery, _mapper);
        }

    }
}
