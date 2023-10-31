﻿using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;


namespace Metadata.Infrastructure.Services.Implementations
{
    public class UnitPriceLandService : IUnitPriceLandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitPriceLandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UnitPriceLandReadDTO> CreateUnitPriceLandAsync(UnitPriceLandWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId)
                ?? throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var landType = await _unitOfWork.AssetGroupRepository.FindAsync(dto.LandTypeId)
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            var unitPriceLand = _mapper.Map<UnitPriceLand>(dto);

            await _unitOfWork.UnitPriceLandRepository.AddAsync(unitPriceLand);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<UnitPriceLandReadDTO>(unitPriceLand);
        }

        public async Task DeleteUnitPriceLand(string unitPriceLandId)
        {
            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(unitPriceLandId);

            if (unitPriceLand == null) throw new EntityWithIDNotFoundException<UnitPriceLand>(unitPriceLand);

            unitPriceLand.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<UnitPriceLandReadDTO> GetUnitPriceLandAsync(string unitPriceLandId)
        {
            return _mapper.Map<UnitPriceLandReadDTO>(await _unitOfWork.UnitPriceLandRepository.FindAsync(unitPriceLandId));
        }

        public async Task<PaginatedResponse<UnitPriceLandReadDTO>> UnitPriceLandQueryAsync(UnitPriceLandQuery query)
        {
            var unitPriceLands = await _unitOfWork.UnitPriceLandRepository.QueryAsync(query);

            return PaginatedResponse<UnitPriceLandReadDTO>.FromEnumerableWithMapping(unitPriceLands, query, _mapper);
        }

        public async Task<UnitPriceLandReadDTO> UpdateUnitPriceLandAsync(string unitPriceLandId, UnitPriceLandWriteDTO dto)
        {
            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(unitPriceLandId);

            if (unitPriceLand == null) throw new EntityWithIDNotFoundException<UnitPriceLand>(unitPriceLand);

            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId)
                ?? throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var landType = await _unitOfWork.AssetGroupRepository.FindAsync(dto.LandTypeId)
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            _mapper.Map(dto, unitPriceLand);

            return _mapper.Map<UnitPriceLandReadDTO>(unitPriceLand);
        }
    }
}