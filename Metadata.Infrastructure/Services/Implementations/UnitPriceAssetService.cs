using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class UnitPriceAssetService : IUnitPriceAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitPriceAssetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UnitPriceAssetReadDTO> CreateUnitPriceAssetAsync(UnitPriceAssetWriteDTO dto)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(dto.AssetUnitId) 
                ?? throw new EntityWithIDNotFoundException<AssetUnit>(dto.AssetUnitId);

            var assetGroup = await _unitOfWork.AssetGroupRepository.FindAsync(dto.AssetGroupId)
                ?? throw new EntityWithIDNotFoundException<AssetGroup>(dto.AssetGroupId);

            var unitPriceAsset = _mapper.Map<UnitPriceAsset>(dto);

            await _unitOfWork.UnitPriceAssetRepository.AddAsync(unitPriceAsset);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<UnitPriceAssetReadDTO>(unitPriceAsset);
        }

        public async Task DeleteUnitPriceAsset(string unitPriceAssetId)
        {
            var unitPriceAsset = await _unitOfWork.UnitPriceAssetRepository.FindAsync(unitPriceAssetId);

            if (unitPriceAsset == null) throw new EntityWithIDNotFoundException<UnitPriceAssetReadDTO>(unitPriceAssetId);

            unitPriceAsset.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<UnitPriceAssetReadDTO> GetUnitPriceAssetAsync(string unitPriceAssetId)
        {
            return _mapper.Map<UnitPriceAssetReadDTO>(await _unitOfWork.UnitPriceAssetRepository.FindAsync(unitPriceAssetId));
        }

        public async Task<PaginatedResponse<UnitPriceAssetReadDTO>> UnitPriceAssetQueryAsync(UnitPriceAssetQuery query)
        {
            var unitPriceAsset = await _unitOfWork.UnitPriceAssetRepository.QueryAsync(query);

            return PaginatedResponse<UnitPriceAssetReadDTO>.FromEnumerableWithMapping(unitPriceAsset, query, _mapper);
        }

        public async Task<UnitPriceAssetReadDTO> UpdateUnitPriceAssetAsync(string unitPriceAssetId, UnitPriceAssetWriteDTO dto)
        {
            var unitPriceAsset = await _unitOfWork.UnitPriceAssetRepository.FindAsync(unitPriceAssetId);

            if (unitPriceAsset == null) throw new EntityWithIDNotFoundException<UnitPriceAsset>(unitPriceAssetId);

            _mapper.Map(dto, unitPriceAsset);

            return _mapper.Map<UnitPriceAssetReadDTO>(unitPriceAsset);

        }
    }
}
