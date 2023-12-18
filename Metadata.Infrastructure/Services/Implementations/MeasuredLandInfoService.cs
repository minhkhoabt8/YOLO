using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class MeasuredLandInfoService : IMeasuredLandInfoService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachFileService _attachFileService;

        public MeasuredLandInfoService(IMapper mapper, IUnitOfWork unitOfWork, IAttachFileService attachFileService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _attachFileService = attachFileService;
        }

        public async Task<MeasuredLandInfoReadDTO> CreateMeasuredLandInfoAsync(MeasuredLandInfoWriteDTO dto)
        {
            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(dto.UnitPriceLandId)
                ?? throw new EntityWithIDNotFoundException<UnitPriceLand>(dto.UnitPriceLandId);

            var gcnLandInfo = await _unitOfWork.GCNLandInfoRepository.FindAsync(dto.GcnLandInfoId) 
                ?? throw new EntityWithIDNotFoundException<GcnlandInfo>(dto.GcnLandInfoId);

            if (gcnLandInfo.OwnerId != dto.OwnerId) throw new InvalidActionException();

            var measuredLandInfo = new MeasuredLandInfo()
            {
                MeasuredPageNumber = dto.MeasuredPageNumber,
                MeasuredPlotNumber = dto.MeasuredPlotNumber,
                MeasuredPlotAddress = dto.MeasuredPlotAddress,
                LandTypeId = dto.LandTypeId,
                MeasuredPlotArea = dto.MeasuredPlotArea,
                WithdrawArea = dto.WithdrawArea,
                CompensationPrice = dto.CompensationPrice,
                CompensationRate = dto.CompensationRate,
                UnitPriceLandCost = dto.UnitPriceLandCost ?? 0,
                CompensationNote = dto.CompensationNote,
                GcnLandInfoId = dto.GcnLandInfoId,
                OwnerId = dto.OwnerId,
                UnitPriceLandId = dto.UnitPriceLandId,
            };

            await _unitOfWork.MeasuredLandInfoRepository.AddAsync(measuredLandInfo);

            if (!dto.AttachFiles.IsNullOrEmpty())
            {
                foreach (var file in dto.AttachFiles!)
                {
                    file.MeasuredLandInfoId = measuredLandInfo.MeasuredLandInfoId;
                }
                await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
            }

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
                var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(item.UnitPriceLandId)
                ?? throw new EntityWithIDNotFoundException<UnitPriceLand>(item.UnitPriceLandId);

                var gcnLandInfo = await _unitOfWork.GCNLandInfoRepository.FindAsync(item.GcnLandInfoId)
                    ?? throw new EntityWithIDNotFoundException<GcnlandInfo>(item.GcnLandInfoId);

                if (gcnLandInfo.OwnerId != item.OwnerId) throw new InvalidActionException();

                var landInfo = new MeasuredLandInfo()
                {
                    MeasuredPageNumber = item.MeasuredPageNumber,
                    MeasuredPlotNumber = item.MeasuredPlotNumber,
                    MeasuredPlotAddress = item.MeasuredPlotAddress,
                    LandTypeId = item.LandTypeId,
                    MeasuredPlotArea = item.MeasuredPlotArea,
                    WithdrawArea = item.WithdrawArea,
                    CompensationPrice = item.CompensationPrice,
                    CompensationRate = item.CompensationRate,
                    UnitPriceLandCost = item.UnitPriceLandCost ?? 0,
                    CompensationNote = item.CompensationNote,
                    GcnLandInfoId = item.GcnLandInfoId,
                    OwnerId = item.OwnerId,
                    UnitPriceLandId = item.UnitPriceLandId,
                };

                landInfo.OwnerId = ownerId;

                await _unitOfWork.MeasuredLandInfoRepository.AddAsync(landInfo);


                foreach(var file in item.AttachFiles!)
                {
                    file.MeasuredLandInfoId = landInfo.MeasuredLandInfoId;
                }

                await _attachFileService.UploadAttachFileAsync(item.AttachFiles!);

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

            measuredLandInfo.OwnerId = null;

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<MeasuredLandInfoReadDTO>> GetAllMeasuredLandInfosAsync()
        {
            var measuredLandInfos = await _unitOfWork.MeasuredLandInfoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MeasuredLandInfoReadDTO>>(measuredLandInfos);
        }

        public async Task<MeasuredLandInfoReadDTO> GetMeasuredLandInfoAsync(string id)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(id, include: "AttachFiles");
            return _mapper.Map<MeasuredLandInfoReadDTO>(measuredLandInfo);
        }

        public async Task<PaginatedResponse<MeasuredLandInfoReadDTO>> MeasuredLandInfoQueryAsync(MeasuredLandInfoQuery query)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.QueryAsync(query);
            return PaginatedResponse<MeasuredLandInfoReadDTO>.FromEnumerableWithMapping(measuredLandInfo, query, _mapper);
        }
        public async Task<MeasuredLandInfoReadDTO> UpdateMeasuredLandInfoAsync(string id, MeasuredLandInfoWriteDTO dto)
        {
            var measuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(id);

            if (measuredLandInfo == null) throw new EntityWithIDNotFoundException<MeasuredLandInfo>(id);

            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(dto.UnitPriceLandId)
               ?? throw new EntityWithIDNotFoundException<MeasuredLandInfo>(dto.UnitPriceLandId);


            if (!dto.GcnLandInfoId.IsNullOrEmpty())
            {
                var gcnLandInfo = await _unitOfWork.GCNLandInfoRepository.FindAsync(dto.GcnLandInfoId)
                ?? throw new EntityWithIDNotFoundException<GcnlandInfo>(dto.GcnLandInfoId);

                if (gcnLandInfo.OwnerId != dto.OwnerId) throw new InvalidActionException("Gcn Land Info And Measured Land Info Owner Mismatch.");

                if(dto.OwnerId != gcnLandInfo.OwnerId || dto.MeasuredPlotNumber!.ToLower() != measuredLandInfo.MeasuredPlotNumber!.ToLower() || dto.MeasuredPlotAddress!.ToLower() != measuredLandInfo.MeasuredPlotAddress!.ToLower())
                {
                    // Verify duplicate MeasuredPlot
                    await VerifyDuplicateMeasuredPlotAsync(dto.OwnerId, dto.MeasuredPlotNumber, dto.MeasuredPageNumber, dto.LandTypeId);
                }

            }

            _mapper.Map(dto, measuredLandInfo);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<MeasuredLandInfoReadDTO>(measuredLandInfo);
        }

        private async Task VerifyDuplicateMeasuredPlotAsync(string ownerId, string measuredPlotNumber, string measuredPageNumber, string landTypeId)
        {
            // Check if there are other owners with the same MeasuredPlotNumber and MeasuredPlotAddress but different LandTypeId
            var hasDifferentLandTypeId = await _unitOfWork.MeasuredLandInfoRepository.HasDuplicateMeasuredPlotAsync(ownerId, measuredPlotNumber, measuredPageNumber, landTypeId);

            if (hasDifferentLandTypeId)
            {
                throw new InvalidOperationException("Một chủ sở hữu khác có cùng số tờ và số thửa nhưng cùng loại đất đã tồn tại.");
            }

            // Check if there are other owners with the same MeasuredPlotNumber and MeasuredPlotAddress
            var hasDuplicatePlotAndAddress = await _unitOfWork.MeasuredLandInfoRepository.HasDuplicateMeasuredPlotAndAddressAsync(ownerId, measuredPlotNumber, measuredPageNumber);

            if (hasDuplicatePlotAndAddress)
            {
                throw new InvalidOperationException("Đã tồn tại một chủ sở hữu khác có cùng số tờ và số thửa.");
            }
        }

        public async Task<MeasuredLandInfoReadDTO?> CheckDuplicateMeasuredLandInfoAsync(string pageNumber, string plotNumber, string? landTypeId = null)
        {
            return _mapper.Map<MeasuredLandInfoReadDTO>(await _unitOfWork.MeasuredLandInfoRepository.CheckDuplicateMeasuredLandInfo(pageNumber, plotNumber, landTypeId));
        }
    }
}
