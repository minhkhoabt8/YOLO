using Amazon.S3.Model;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    
    public class GCNLandInfoService : IGCNLandInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachFileService _attachFileService;
        private readonly IUploadFileService _uploadFileService;

        public GCNLandInfoService(IUnitOfWork unitOfWork, IMapper mapper, IAttachFileService attachFileService, IUploadFileService uploadFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachFileService = attachFileService;
            _uploadFileService = uploadFileService;
        }

        public async Task<GCNLandInfoReadDTO> CreateGCNLandInfoAsync(GCNLandInfoWriteDTO dto)
        {

            var ownerId = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId)
                ?? throw new EntityWithIDNotFoundException<Core.Entities.Owner>(dto.OwnerId);

            var landType = await _unitOfWork.LandTypeRepository.FindAsync(dto.LandTypeId) 
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            //var gcnLandInfo = _mapper.Map<GcnlandInfo>(dto);
            var gcnLandInfo = new GcnlandInfo()
            {
                GcnLandInfoId = Guid.NewGuid().ToString(),
                GcnPageNumber = dto.GcnPageNumber,
                GcnPlotNumber = dto.GcnPlotNumber,
                GcnPlotAddress = dto.GcnPlotAddress,
                LandTypeId = dto.LandTypeId,
                GcnPlotArea = dto.GcnPlotArea,
                GcnOwnerCertificate = dto.GcnOwnerCertificate,
                OwnerId = dto.OwnerId,
                IsDeleted = false
            };

            await _unitOfWork.GCNLandInfoRepository.AddAsync(gcnLandInfo);

            if(!dto.MeasuredLandInfos.IsNullOrEmpty())
            {
                foreach(var measuredDto in dto.MeasuredLandInfos!)
                {
                    var measuredLandInfo = new MeasuredLandInfo()
                    {
                        MeasuredLandInfoId =  Guid.NewGuid().ToString(),
                        MeasuredPageNumber = measuredDto.MeasuredPageNumber,
                        MeasuredPlotNumber= measuredDto.MeasuredPlotNumber,
                        MeasuredPlotAddress = measuredDto.MeasuredPlotAddress,
                        LandTypeId = measuredDto.LandTypeId,
                        MeasuredPlotArea= measuredDto.MeasuredPlotArea,
                        WithdrawArea = measuredDto.WithdrawArea,
                        CompensationPrice = measuredDto.CompensationPrice,
                        CompensationRate = measuredDto.CompensationRate,
                        CompensationNote = measuredDto.CompensationNote,
                        UnitPriceLandCost= measuredDto.UnitPriceLandCost ?? 0,
                        GcnLandInfoId = measuredDto.GcnLandInfoId!,
                        OwnerId = measuredDto.OwnerId!,
                        UnitPriceLandId = measuredDto.UnitPriceLandId,
                        IsDeleted = false

                    };
                    await _unitOfWork.MeasuredLandInfoRepository.AddAsync(measuredLandInfo);
                    foreach (var item in measuredDto.AttachFiles!)
                    {
                        item.MeasuredLandInfoId = measuredLandInfo.MeasuredLandInfoId;
                    }
                    await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
                }
            }

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
                //var ownerId = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId)
                //?? throw new EntityWithIDNotFoundException<Owner>(dto.OwnerId);

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
                       
                        var measuredLandInfo = _mapper.Map<MeasuredLandInfo>(item);

                        measuredLandInfo.OwnerId = gcnLandInfo.OwnerId;

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
            var gcnLandInfo = await _unitOfWork.GCNLandInfoRepository.FindAsync(id, include: "MeasuredLandInfos");

            if (gcnLandInfo == null) throw new EntityWithIDNotFoundException<GcnlandInfo>(id);

            foreach(var item in gcnLandInfo.MeasuredLandInfos)
            {
                if (!item.IsDeleted)
                {
                    throw new InvalidActionException("Không thể xóa GCN đất.");
                }
            }

            gcnLandInfo.IsDeleted = true;

            gcnLandInfo.OwnerId = null;

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
            var gcnLandInfo = await _unitOfWork.GCNLandInfoRepository.FindAsync(id);

            if (gcnLandInfo == null) throw new EntityWithIDNotFoundException<GcnlandInfo>(id);

            var ownerId = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId)
                ?? throw new EntityWithIDNotFoundException<Core.Entities.Owner>(dto.OwnerId);

            var landType = await _unitOfWork.LandTypeRepository.FindAsync(dto.LandTypeId)
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            _mapper.Map(dto, gcnLandInfo);

            if (!dto.AttachFiles.IsNullOrEmpty())
            {
                foreach(var item in dto.AttachFiles!)
                {
                    item.GcnLandInfoId = id;
                }

                await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<GCNLandInfoReadDTO>(gcnLandInfo);
        }

        public async Task<IEnumerable<GCNLandInfoReadDTO>> CreateOwnerGcnLandInfosAsync(string ownerId, IEnumerable<GCNLandInfoWriteDTO> dto)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Core.Entities.Owner>(ownerId);

            if (dto == null) throw new InvalidActionException(nameof(dto));

            var landInfoList = new List<GcnlandInfo>();

            foreach (var item in dto)
            {
                var landInfo = _mapper.Map<GcnlandInfo>(item);

                landInfo.OwnerId = ownerId;

                
                foreach (var file in item.AttachFiles!)
                {
                    file.GcnLandInfoId = landInfo.GcnLandInfoId;
                }

                await _attachFileService.UploadAttachFileAsync(item.AttachFiles!);

                if (!landInfo.MeasuredLandInfos.IsNullOrEmpty())
                {
                    
                    foreach (var measuredLandInfo in landInfo.MeasuredLandInfos!)
                    {

                        measuredLandInfo.OwnerId = ownerId;

                        if (measuredLandInfo.GcnLandInfoId.IsNullOrEmpty())
                        {
                            measuredLandInfo.GcnLandInfoId = landInfo.GcnLandInfoId;
                        }

                        if (!item.AttachFiles.IsNullOrEmpty())
                        {
                            foreach (var file in measuredLandInfo.AttachFiles!)
                            {
                                file.MeasuredLandInfoId = measuredLandInfo.MeasuredLandInfoId;
                            }

                            await _attachFileService.UploadAttachFileAsync(item.AttachFiles!);
                        }

                        landInfoList.ForEach(item => item.MeasuredLandInfos.Add(measuredLandInfo));
                    }
                }
                
                await _unitOfWork.GCNLandInfoRepository.AddAsync(landInfo);

                landInfoList.Add(landInfo);
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<GCNLandInfoReadDTO>>(landInfoList);
        }

        public async Task<GCNLandInfoReadDTO> CheckDuplicateGCNLandInfoAsync(string pageNumber, string plotNumber)
        {
            return _mapper.Map<GCNLandInfoReadDTO>(await _unitOfWork.GCNLandInfoRepository.CheckDuplicateGCNLandInfo(pageNumber, plotNumber));
        }
    }
}
