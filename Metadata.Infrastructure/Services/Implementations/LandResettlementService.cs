using Amazon.S3.Model;
using Aspose.Pdf.Operators;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.LandResettlement;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;


namespace Metadata.Infrastructure.Services.Implementations
{
    public class LandResettlementService : ILandResettlementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LandResettlementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LandResettlementReadDTO> CreateLandResettlementAsync(LandResettlementWriteDTO dto)
        {
            if (!dto.ResettlementProjectId.IsNullOrEmpty())
            {
                var resettlementProject = await _unitOfWork.ResettlementProjectRepository.FindAsync(dto.ResettlementProjectId!)
                 ?? throw new EntityWithIDNotFoundException<ResettlementProject>(dto.ResettlementProjectId!);
            }
            if (!dto.OwnerId.IsNullOrEmpty())
            {
                var owner = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId!, include: "LandResettlements", trackChanges:false)
                 ?? throw new EntityWithIDNotFoundException<Core.Entities.Owner>(dto.OwnerId!);

                var ownerProject = await _unitOfWork.ProjectRepository.FindAsync(owner.ProjectId!)
                    ?? throw new EntityWithIDNotFoundException<Project>(owner.ProjectId!);

                if (ownerProject.IsDeleted)
                {
                    throw new InvalidActionException("Không thể thêm đất tái định cư vào dự án đã bị xóa.");
                }

                //check if Project Resettlent still have enough land to resettlemt

                var numberofResettlementOwner = await _unitOfWork.OwnerRepository.GetTotalLandResettlementsOfOwnersInProjectAsync(ownerProject.ProjectId);

                if (ownerProject.ResettlementProject!.LandNumber <= numberofResettlementOwner + owner.LandResettlements.Count())
                {
                    throw new InvalidActionException($"Không thể thêm mới đất tái định cư cho chủ sở hữu: [{owner.OwnerCode}] vì đã vượt quá số lượng lô đất tái định cư: [{ownerProject.ResettlementProject.LandNumber}] có trong dự án: [{ownerProject.ProjectCode}].");
                }

            }

            var resettlement = _mapper.Map<LandResettlement>(dto);

            await _unitOfWork.LandResettlementRepository.AddAsync(resettlement);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandResettlementReadDTO>(resettlement);

        }

        public Task<IEnumerable<LandResettlementReadDTO>> CreateLandResettlementsAsync(IEnumerable<LandResettlementWriteDTO> dto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLandResettlementAsync(string id)
        {
            var landResettlement = await _unitOfWork.LandResettlementRepository.FindAsync(id)
                ?? throw new EntityWithIDNotFoundException<LandResettlement>(id);

            if (landResettlement.OwnerId != null) throw new InvalidActionException("Không thể xóa Đất tái định cư đã tồn tại Chủ sở hữu.");

            _unitOfWork.LandResettlementRepository.Delete(landResettlement);

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<LandResettlementReadDTO>> GetAllLandResettlementsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<LandResettlementReadDTO> GetLandResettlementAsync(string id)
        {
            var landResettlement = await _unitOfWork.LandResettlementRepository.FindAsync(id, include: "ResettlementProject, Owner")
                ?? throw new EntityWithIDNotFoundException<LandResettlement>(id);
            return _mapper.Map<LandResettlementReadDTO>(landResettlement);
        }

        public async Task<IEnumerable<LandResettlementReadDTO>> GetLandResettlementsOfOwnerAsync(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                ?? throw new EntityWithIDNotFoundException<Core.Entities.Owner>(ownerId);

            return _mapper.Map<IEnumerable<LandResettlementReadDTO>>(await _unitOfWork.LandResettlementRepository.GetLandResettlementsOfOwnerIncludeResettlementProjectAsync(ownerId));
        }

        public async Task<IEnumerable<LandResettlementReadDTO>> GetLandResettlementsOfResettlementProjectAsync(string resettlementProjectId)
        {
            var resetlementProject  = await _unitOfWork.ResettlementProjectRepository.FindAsync(resettlementProjectId)
                ?? throw new EntityWithIDNotFoundException<LandResettlement>(resettlementProjectId);

            return _mapper.Map<IEnumerable<LandResettlementReadDTO>>(await _unitOfWork.LandResettlementRepository.GetLandResettlementsOfResettlementProjectIncludeOwnerAsync(resettlementProjectId));
        }

        public async Task<LandResettlementReadDTO> UpdateLandResettlementAsync(string id, LandResettlementWriteDTO dto)
        {
            var landResettlement = await _unitOfWork.LandResettlementRepository.FindAsync(id);

            if (landResettlement == null) throw new EntityWithIDNotFoundException<LandResettlement>(id);

            if(landResettlement.PageNumber != dto.PageNumber || landResettlement.PlotNumber != dto.PlotNumber)
            {
                var duplicateLandResettlement = await _unitOfWork.LandResettlementRepository.CheckDuplicateLandResettlement(dto.PageNumber!, dto.PlotNumber!) ??
                    throw new UniqueConstraintException($"Có một đất tái định cư với số tờ {dto.PageNumber} và số thửa {dto.PlotNumber} khác đã tồn tại trong hệ thống.");
            }

            _mapper.Map(dto, landResettlement);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandResettlementReadDTO>(landResettlement);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<decimal> CalculateOwnerTotalLandResettlementPriceInPlanAsync(string planId)
        {
            return await _unitOfWork.LandResettlementRepository.CalculateOwnerTotalLandResettlementPriceInPlanAsync(planId);
        }

        public async Task<LandResettlementReadDTO> CheckDuplicateLandResettlementAsync(string pageNumber, string plotNumber)
        {
            var landResettlemtnt = await _unitOfWork.LandResettlementRepository.CheckDuplicateLandResettlement(pageNumber!, plotNumber!)
                ?? throw new UniqueConstraintException($"Có một đất tái định cư với số tờ {pageNumber} và số thửa {plotNumber} khác đã tồn tại trong hệ thống.");
            return _mapper.Map<LandResettlementReadDTO>(landResettlemtnt);
        }
    }
}
