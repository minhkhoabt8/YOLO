using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class MeasuredLandInfoService : IMeasuredLandInfoService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MeasuredLandInfoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MeasuredLandInfoReadDTO> CreateMeasuredLandInfoAsync(MeasuredLandInfoWriteDTO dto)
        {
            //TODO: Validate IDs

            var measuredLandInfo = _mapper.Map<MeasuredLandInfo>(dto);

            await _unitOfWork.MeasuredLandInfoRepository.AddAsync(measuredLandInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<MeasuredLandInfoReadDTO>(measuredLandInfo);
        }

        public Task CreateMeasuredLandInfoFromFileAsync(IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MeasuredLandInfoReadDTO>> CreateOwnerMeasuredLandInfosAsync(string ownerId, IEnumerable<MeasuredLandInfoWriteDTO> dto)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (dto == null) throw new InvalidActionException(nameof(dto));

            var landInfoList = new List<MeasuredLandInfo>();

            foreach (var item in dto)
            {
                var landInfo = _mapper.Map<MeasuredLandInfo>(item);

                landInfo.OwnerId = ownerId;

                await _unitOfWork.MeasuredLandInfoRepository.AddAsync(landInfo);

                landInfoList.Add(landInfo);
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<MeasuredLandInfoReadDTO>>(landInfoList);
        }

        public async Task DeleteMeasuredLandInfoAsync(string id)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(id);

            if (measuredLandInfo == null) throw new EntityWithIDNotFoundException<MeasuredLandInfo>(id);

            measuredLandInfo.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<MeasuredLandInfoReadDTO>> GetAllMeasuredLandInfosAsync()
        {
            var measuredLandInfos = await _unitOfWork.MeasuredLandInfoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MeasuredLandInfoReadDTO>>(measuredLandInfos);
        }

        public async Task<MeasuredLandInfoReadDTO> GetMeasuredLandInfoAsync(string id)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(id);
            return _mapper.Map<MeasuredLandInfoReadDTO>(measuredLandInfo);
        }

        public async Task<PaginatedResponse<MeasuredLandInfoReadDTO>> MeasuredLandInfoQueryAsync(MeasuredLandInfoQuery query)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.QueryAsync(query);
            return PaginatedResponse<MeasuredLandInfoReadDTO>.FromEnumerableWithMapping(measuredLandInfo, query, _mapper);
        }

        public async Task<MeasuredLandInfoReadDTO> UpdateMeasuredLandInfoAsync(string id, MeasuredLandInfoWriteDTO dto)
        {
            //TODO: Validate IDs
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(id);

            if (measuredLandInfo == null) throw new EntityWithIDNotFoundException<MeasuredLandInfo>(id);

            _mapper.Map(dto, measuredLandInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<MeasuredLandInfoReadDTO>(measuredLandInfo);
        }
    }
}
