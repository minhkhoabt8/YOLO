using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
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
    public class LandPositionInfoService : ILandPositionInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LandPositionInfoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LandPositionInfoReadDTO> CreateLandPositionInfoAsync(LandPositionInfoWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId!)
                ?? throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var landPositionInfo = _mapper.Map<LandPositionInfo>(dto);

            await _unitOfWork.LandPositionInfoRepository.AddAsync(landPositionInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandPositionInfoReadDTO>(landPositionInfo);
        }

        public async Task DeleteLandPositionInfo(string unitPriceLandId)
        {
            var landPositionInfo = await _unitOfWork.LandPositionInfoRepository.FindAsync(unitPriceLandId);

            if (landPositionInfo == null) throw new EntityWithIDNotFoundException<LandPositionInfo>(unitPriceLandId);

            _unitOfWork.LandPositionInfoRepository.Delete(landPositionInfo);

            await _unitOfWork.CommitAsync();
        }

        public async Task<LandPositionInfoReadDTO> GetLandPositionInfoAsync(string id)
        {
            return _mapper.Map<LandPositionInfoReadDTO>(await _unitOfWork.LandPositionInfoRepository.FindAsync(id));
        }

        public async Task<PaginatedResponse<LandPositionInfoReadDTO>> LandPositionInfoQueryAsync(LandPositionInfoQuery query)
        {
            var landPositionInfos = await _unitOfWork.LandPositionInfoRepository.QueryAsync(query);

            return PaginatedResponse<LandPositionInfoReadDTO>.FromEnumerableWithMapping(landPositionInfos, query, _mapper);
        }

        public async Task<LandPositionInfoReadDTO> UpdateLandPositionInfoAsync(string id, LandPositionInfoWriteDTO dto)
        {
            var landPositionInfo = await _unitOfWork.LandPositionInfoRepository.FindAsync(id);

            if (landPositionInfo == null) throw new EntityWithIDNotFoundException<LandPositionInfo>(id);

            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId!)
                ?? throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            _mapper.Map(dto, landPositionInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandPositionInfoReadDTO>(landPositionInfo);
        }
    }
}
