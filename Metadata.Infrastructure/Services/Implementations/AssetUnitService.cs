using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
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
            
            _mapper.Map(assetUnitUpdateDTO, existAssetUnit);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetUnitReadDTO>(existAssetUnit);
        }

        public async Task<AssetUnitReadDTO?> CreateAssetUnitAsync(AssetUnitWriteDTO assetUnitWriteDTO)
        {
            await EnsureAssetUnitCodeNotDuplicate(assetUnitWriteDTO.Code);
            var assetUnit = _mapper.Map<AssetUnit>(assetUnitWriteDTO);
            await _unitOfWork.AssetUnitRepository.AddAsync(assetUnit);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AssetUnitReadDTO>(assetUnit);
        }

        private async Task EnsureAssetUnitCodeNotDuplicate (string code)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(code);
            if (assetUnit != null && assetUnit.Code == code)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(assetUnit.Code), code);
            }
        }

       
    }
}
