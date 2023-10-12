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
            EnsureAssetGroupCodeNotDupicate(assetGroupWriteDTO.Code);
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

        public async Task<AssetGroupReadDTO> GetAssetGroupAsync(string code)
        {
            var landgroups = await _unitOfWork.AssetGroupRepository.FindAsync(code);
            return _mapper.Map<AssetGroupReadDTO>(landgroups);
        }

        public async Task<AssetGroupReadDTO> UpdateAssetGroupAsync(string id, AssetGroupWriteDTO assetGroupWriteDTO)
        {
            var existAssetGroup = _unitOfWork.AssetGroupRepository.FindAsync(id);
            if (existAssetGroup == null)
            {
                throw new EntityWithIDNotFoundException<AssetGroup>(id);
            }
              _mapper.Map(assetGroupWriteDTO, existAssetGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetGroupReadDTO>(existAssetGroup);
        }
        private async Task EnsureAssetGroupCodeNotDupicate(string code)
        {
            var assetGroup = await _unitOfWork.AssetGroupRepository.FindByCodeAsync(code);
            if (assetGroup != null && assetGroup.Code == code)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetGroup.Code), code);
            }
        }
    }
}
