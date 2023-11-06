using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
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
    public class AssetUnitService : IAssetUnitService
    {
        private IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public AssetUnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(string delete)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(delete);
            if (assetUnit == null)
            {
                throw new EntityWithIDNotFoundException<AssetUnit>(delete);
            }
            assetUnit.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;

        }

        public async Task<IEnumerable<AssetUnitReadDTO>> GetActivedAssetUnitAsync()
        {
            var assetUnits = await _unitOfWork.AssetUnitRepository.GetActivedAssetUnitAsync();
            return _mapper.Map<IEnumerable<AssetUnitReadDTO>>(assetUnits);
        }

        public async Task<IEnumerable<AssetUnitReadDTO>> GetAllAssetUnitAsync()
        {
            var assetUnits = await _unitOfWork.AssetUnitRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AssetUnitReadDTO>>(assetUnits);
        }   

        public async Task<AssetUnitReadDTO?> GetAsync(string code)
        {
            var assetUnits = await _unitOfWork.AssetUnitRepository.FindAsync(code);
            return _mapper.Map<AssetUnitReadDTO>(assetUnits);
        }   

        public async Task<AssetUnitReadDTO?> UpdateAsync(string id, AssetUnitWriteDTO assetUnitUpdateDTO)
        {
            var existAssetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(id);
            if (existAssetUnit == null)
            {
                throw new EntityWithIDNotFoundException<AssetUnit>(id);
            }
            await EnsureAssetUnitCodeNotDuplicateForUpdate(assetUnitUpdateDTO.Code, assetUnitUpdateDTO.Name,  id);
            _mapper.Map(assetUnitUpdateDTO, existAssetUnit);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetUnitReadDTO>(existAssetUnit);
        }

        public async Task<AssetUnitReadDTO?> CreateAssetUnitAsync(AssetUnitWriteDTO assetUnitWriteDTO)
        {
            await EnsureAssetUnitCodeNotDuplicate(assetUnitWriteDTO.Code, assetUnitWriteDTO.Name);
            var assetUnit = _mapper.Map<AssetUnit>(assetUnitWriteDTO);
            await _unitOfWork.AssetUnitRepository.AddAsync(assetUnit);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetUnitReadDTO>(assetUnit);
        }

        public async Task<IEnumerable<AssetUnitReadDTO>> CreateListAssetUnitAsync(IEnumerable<AssetUnitWriteDTO> assetUnitWriteDTOs)
        {
            var assetUnits = new List<AssetUnitReadDTO>();
            foreach (var item in assetUnitWriteDTOs)
            {
                await EnsureAssetUnitCodeNotDuplicate(item.Code, item.Name);
                var assetUnit = _mapper.Map<AssetUnit>(item);
                await _unitOfWork.AssetUnitRepository.AddAsync(assetUnit);
                await _unitOfWork.CommitAsync();
                var assetUnitRead = _mapper.Map<AssetUnitReadDTO>(assetUnit);
                assetUnits.Add(assetUnitRead);

            }
            return assetUnits; 
        }

        private async Task EnsureAssetUnitCodeNotDuplicate(string code , string name)
        {
             var assetUnit = await _unitOfWork.AssetUnitRepository.FindByCodeAndIsDeletedStatus(code,false);
            if (assetUnit != null && assetUnit.Code == code)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(assetUnit.Code), code);
            }
            var assetUnitt = await _unitOfWork.AssetUnitRepository.FindByNameAndIsDeletedStatus(name,false);
            if (assetUnitt != null && assetUnitt.Name == name)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(assetUnit.Name), name);
            }
        }

        public async Task CheckNameAssetUnitNotDuplicate(string name)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindByNameAndIsDeletedStatus(name,false);
            if (assetUnit != null && assetUnit.Name == name)
            {
                throw new UniqueConstraintException<AssetUnit>(nameof(assetUnit.Name), name);
            }
        }

        public async Task CheckCodeAssetUnitNotDuplicate(string code)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindByCodeAndIsDeletedStatus(code,false);
            if (assetUnit != null && assetUnit.Code == code)
            {
                throw new UniqueConstraintException<AssetUnit>(nameof(assetUnit.Code), code);
            }
        }

        
        public async Task<PaginatedResponse<AssetUnitReadDTO>> QueryAssetUnitAsync(AssetUnitQuery paginationQuery)
        {
            var assetUnits = await _unitOfWork.AssetUnitRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<AssetUnitReadDTO>.FromEnumerableWithMapping(assetUnits,paginationQuery, _mapper);
        }

        private async Task EnsureAssetUnitCodeNotDuplicateForUpdate(string code, string name, string id)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindByCodeAndIsDeletedStatusForUpdate(code, id, false);
            if (assetUnit != null && assetUnit.Code == code && assetUnit.AssetUnitId != id)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetUnit.Code), code);
            }
            var assetUnitByName = await _unitOfWork.AssetUnitRepository.FindByCodeAndIsDeletedStatusForUpdate(name, id, false);
            if (assetUnitByName != null && assetUnitByName.Name == name && assetUnitByName.AssetUnitId != id)
            {
                throw new UniqueConstraintException<AssetGroup>(nameof(assetUnitByName.Name), name);
            }
        }
    }
}
