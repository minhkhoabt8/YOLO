using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;


namespace Metadata.Infrastructure.Services.Implementations
{
    
    public class GCNLandInfoService : IGCNLandInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachFileService _attachFileService;

        public GCNLandInfoService(IUnitOfWork unitOfWork, IMapper mapper, IAttachFileService attachFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachFileService = attachFileService;
        }

        public async Task<GCNLandInfoReadDTO> CreateGCNLandInfoAsync(GCNLandInfoWriteDTO dto)
        {

            var ownerId = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(dto.OwnerId);

            var landType = await _unitOfWork.LandTypeRepository.FindAsync(dto.LandTypeId) 
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            var gcnLandInfo = _mapper.Map<GcnlandInfo>(dto);

            await _unitOfWork.GCNLandInfoRepository.AddAsync(gcnLandInfo);

            if (!dto.AttachFiles.IsNullOrEmpty())
            {
                foreach(var item in dto.AttachFiles!)
                {
                    item.GcnLandInfoId = gcnLandInfo.GcnLandInfoId;
                }
                await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<GCNLandInfoReadDTO>(gcnLandInfo);
        }

        public async Task<IEnumerable<GCNLandInfoReadDTO>> CreateGCNLandInfosAsync(IEnumerable<GCNLandInfoWriteDTO> dtos)
        {
            var gcnList = new List<GcnlandInfo>();

            foreach(var dto in dtos)
            {
                var ownerId = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(dto.OwnerId);

                var landType = await _unitOfWork.LandTypeRepository.FindAsync(dto.LandTypeId)
                    ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

                var gcnLandInfo = _mapper.Map<GcnlandInfo>(dto);

                await _unitOfWork.GCNLandInfoRepository.AddAsync(gcnLandInfo);

                if (!dto.AttachFiles.IsNullOrEmpty())
                {
                    foreach (var item in dto.AttachFiles!)
                    {
                        item.GcnLandInfoId = gcnLandInfo.GcnLandInfoId;
                    }
                    await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
                }

                if (!dto.MeasuredLandInfos.IsNullOrEmpty())
                {
                    
                    foreach(var item in dto.MeasuredLandInfos!)
                    {
                        //check if GCN Land Info And Measured Has the same owner
                        if (item.OwnerId != dto.OwnerId) 
                        {
                            throw new InvalidActionException();
                        }

                        var measuredLandInfo = _mapper.Map<MeasuredLandInfo>(item);

                        await _unitOfWork.MeasuredLandInfoRepository.AddAsync(measuredLandInfo);

                        if (!item.AttachFiles.IsNullOrEmpty())
                        {
                            foreach (var file in dto.AttachFiles!)
                            {
                                file.MeasuredLandInfoId = measuredLandInfo.MeasuredLandInfoId;
                            }
                            await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
                        }

                        gcnList.ForEach(item => item.MeasuredLandInfos.Add(measuredLandInfo));
                    }
                }

                gcnList.Add(gcnLandInfo);

                await _unitOfWork.CommitAsync();
            }

            return _mapper.Map<IEnumerable<GCNLandInfoReadDTO>>(gcnList);
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

            var ownerId = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(dto.OwnerId);

            var landType = await _unitOfWork.LandTypeRepository.FindAsync(dto.LandTypeId)
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            _mapper.Map(dto, measuredLandInfo);

            if (!dto.AttachFiles.IsNullOrEmpty())
            {
                foreach(var item in dto.AttachFiles!)
                {
                    item.GcnLandInfoId = id;
                }

                await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<GCNLandInfoReadDTO>(measuredLandInfo);
        }

        public async Task<IEnumerable<GCNLandInfoReadDTO>> CreateOwnerGcnLandInfosAsync(string ownerId, IEnumerable<GCNLandInfoWriteDTO> dto)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (dto == null) throw new InvalidActionException(nameof(dto));

            var landInfoList = new List<GcnlandInfo>();

            foreach (var item in dto)
            {
                var landInfo = _mapper.Map<GcnlandInfo>(item);

                landInfo.OwnerId = ownerId;

                await _unitOfWork.GCNLandInfoRepository.AddAsync(landInfo);

                foreach (var file in item.AttachFiles!)
                {
                    file.GcnLandInfoId = landInfo.GcnLandInfoId;
                }

                await _attachFileService.UploadAttachFileAsync(item.AttachFiles!);

                landInfoList.Add(landInfo);
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<GCNLandInfoReadDTO>>(landInfoList);
        }

    }
}
