using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IGCNLandInfoService _gcNLandInfoService;
        private readonly IMeasuredLandInfoService _measuredLandInfoService;
        private readonly ISupportService _supportService;
        private readonly IDeductionService _deductionService;
        private readonly IAssetCompensationService _assetCompensationService;
        private readonly IAttachFileService _attachFileService;

        public OwnerService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IGCNLandInfoService gcNLandInfoService, IMeasuredLandInfoService measuredLandInfoService, ISupportService supportService, IDeductionService deductionService, IAssetCompensationService assetCompensationService, IAttachFileService attachFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _gcNLandInfoService = gcNLandInfoService;
            _measuredLandInfoService = measuredLandInfoService;
            _supportService = supportService;
            _deductionService = deductionService;
            _assetCompensationService = assetCompensationService;
            _attachFileService = attachFileService;
        }

        public async Task<IEnumerable<OwnerReadDTO>> GetAllOwner()
        {
            var owners = await _unitOfWork.OwnerRepository.GetAllOwner();
            return _mapper.Map<IEnumerable<OwnerReadDTO>>(owners);
        }

        public async Task<OwnerReadDTO> CreateOwnerAsync(OwnerWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(dto.PlanId);

            var owner = _mapper.Map<Owner>(dto);

            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.OwnerRepository.AddAsync(owner);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }


        public async Task<OwnerReadDTO> CreateOwnerWithFullInfomationAsync(OwnerWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId);

            if(plan == null) throw new EntityWithIDNotFoundException<Plan>(dto.PlanId);

            //1.Add Owner
            var owner = _mapper.Map<Owner>(dto);

            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.OwnerRepository.AddAsync(owner);

            await _unitOfWork.CommitAsync();

            var ownerReadDto = _mapper.Map<OwnerReadDTO>(owner);

            //2.Add Others Info
            //2.1 Add Supports
            if (!dto.Supports.IsNullOrEmpty())
            {
               ownerReadDto.Supports = await _supportService.CreateOwnerSupportsAsync(owner.OwnerId, dto.Supports!);
            }
            //2.2 Add Deductions
            if(!dto.Deductions.IsNullOrEmpty())
            {
                ownerReadDto.Deductions = await _deductionService.CreateOwnerDeductionsAsync(owner.OwnerId, dto.Deductions!);
            }
            //2.3 Gcn Land Info
            if(!dto.GcnlandInfos.IsNullOrEmpty())
            {
                ownerReadDto.GcnlandInfos =  await _gcNLandInfoService.CreateOwnerGcnLandInfosAsync(owner.OwnerId, dto.GcnlandInfos!);
            }
            //2.4 MeasuredLandInfos
            if(!dto.MeasuredLandInfos.IsNullOrEmpty())
            {
                ownerReadDto.MeasuredLandInfos = await _measuredLandInfoService.CreateOwnerMeasuredLandInfosAsync(owner.OwnerId, dto.MeasuredLandInfos!);
            }
            //2.5 AssetCompensations
            if (!dto.AssetCompensations.IsNullOrEmpty())
            {
                ownerReadDto.AssetCompensations = await _assetCompensationService.CreateOwnerAssetCompensationsAsync(owner.OwnerId, dto.AssetCompensations!);
            }

            //2.6 AttachFiles
            if(!dto.AttachFiles.IsNullOrEmpty())
            {
                ownerReadDto.AttachFiles = await _attachFileService.CreateOwnerAttachFilesAsync(owner.OwnerId, dto.AttachFiles!);
            }
            return ownerReadDto;
        }



        /// <summary>
        /// This method only used to test export all owners in database
        /// </summary>
        /// <returns></returns>
        public async Task<ExportFileDTO> ExportOwnerFileAsync(string projectId)
        {
            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfProjectAsync(projectId);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Owners");

                int row = 1;

                var properties = typeof(Owner).GetProperties();

                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = properties[col].Name;
                }

                foreach (var item in owners)
                {
                    row++;
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = properties[col].GetValue(item);
                    }
                }
                return new ExportFileDTO
                {
                    FileByte = package.GetAsByteArray(),
                    FileName = $"{"yolo" + $"{Guid.NewGuid()}"}"
                };
            }
        }

        public async Task DeleteOwner(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            _unitOfWork.OwnerRepository.Delete(owner);

            await _unitOfWork.CommitAsync();
        }

        public async Task<OwnerReadDTO> GetOwnerAsync(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);
            return _mapper.Map<OwnerReadDTO>(owner);
        }

        public Task ImportOwner(IFormFile attachFile)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResponse<OwnerReadDTO>> QueryOwnerAsync(OwnerQuery query)
        {
           var owner = await _unitOfWork.OwnerRepository.QueryAsync(query);
           return PaginatedResponse<OwnerReadDTO>.FromEnumerableWithMapping(owner, query, _mapper);
        }

        public async Task<OwnerReadDTO> UpdateOwnerAsync(string ownerId, OwnerWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(dto.PlanId);

            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            _mapper.Map(dto, owner);

            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }

        public async Task<IEnumerable<OwnerReadDTO>> GetOwnersOfProjectAsync(string projectId)
        {
            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfProjectAsync(projectId);

            return _mapper.Map<IEnumerable<OwnerReadDTO>>(owners);
        }

        public async Task<OwnerReadDTO> AssignProjectOwnerAsync(string projectId, string ownerId)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(projectId);

            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            owner.ProjectId = projectId;

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);

        }

        
    }
}
