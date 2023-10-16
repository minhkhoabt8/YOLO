using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;


namespace Metadata.Infrastructure.Services.Implementations
{
    
    public class GCNLandInfoService : IGCNLandInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GCNLandInfoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GCNLandInfoReadDTO> CreateGCNLandInfoAsync(GCNLandInfoWriteDTO dto)
        {
            //TODO: Validate IDs

            var gcnLandInfo = _mapper.Map<GcnlandInfo>(dto);

            await _unitOfWork.GCNLandInfoRepository.AddAsync(gcnLandInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<GCNLandInfoReadDTO>(gcnLandInfo);
        }

        public async Task DeleteGCNLandInfoAsync(string id)
        {
            var gcnLandInfo = await _unitOfWork.GCNLandInfoRepository.FindAsync(id);

            if (gcnLandInfo == null) throw new EntityWithIDNotFoundException<GcnlandInfo>(id);

            gcnLandInfo.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<PaginatedResponse<GCNLandInfoReadDTO>> GCNLandInfoQueryAsync(GCNLandInfoQuery query)
        {
            var gcnLandInfos = await _unitOfWork.GCNLandInfoRepository.QueryAsync(query);
            return PaginatedResponse<GCNLandInfoReadDTO>.FromEnumerableWithMapping(gcnLandInfos, query, _mapper);
        }

        public async Task<IEnumerable<GCNLandInfoReadDTO>> GetAllGCNLandInfosAsync()
        {
            var gcnLandInfos = await _unitOfWork.GCNLandInfoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GCNLandInfoReadDTO>>(gcnLandInfos);
        }

        public async Task<GCNLandInfoReadDTO> GetGCNLandInfoAsync(string id)
        {
            var gcnLandInfos = await _unitOfWork.GCNLandInfoRepository.FindAsync(id);
            return _mapper.Map<GCNLandInfoReadDTO>(gcnLandInfos);
        }

        public async Task<GCNLandInfoReadDTO> UpdateGCNLandInfoAsync(string id, GCNLandInfoWriteDTO dto)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(id);

            if (measuredLandInfo == null) throw new EntityWithIDNotFoundException<GcnlandInfo>(id);

            _mapper.Map(dto, measuredLandInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<GCNLandInfoReadDTO>(measuredLandInfo);
        }
    }
}
