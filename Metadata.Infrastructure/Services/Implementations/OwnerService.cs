using Amazon.S3.Model;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using Owner = Metadata.Core.Entities.Owner;

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
        private readonly IUploadFileService _uploadFileService;

        public OwnerService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IGCNLandInfoService gcNLandInfoService, IMeasuredLandInfoService measuredLandInfoService, ISupportService supportService, IDeductionService deductionService, IAssetCompensationService assetCompensationService, IAttachFileService attachFileService, IUploadFileService uploadFileService)
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
            _uploadFileService = uploadFileService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OwnerReadDTO>> GetAllOwner()
        {
            var owners = await _unitOfWork.OwnerRepository.GetAllOwner();
            return _mapper.Map<IEnumerable<OwnerReadDTO>>(owners);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithIDNotFoundException{Project}"></exception>
        /// <exception cref="EntityWithIDNotFoundException{Plan}"></exception>
        /// <exception cref="CanNotAssignUserException"></exception>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithIDNotFoundException{Project}"></exception>
        /// <exception cref="EntityWithIDNotFoundException{Plan}"></exception>
        /// <exception cref="EntityWithIDNotFoundException{OrganizationType}"></exception>
        /// <exception cref="CanNotAssignUserException"></exception>
        public async Task<OwnerReadDTO> CreateOwnerWithFullInfomationAsync1(OwnerWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            if (!dto.PlanId.IsNullOrEmpty())
            {
                var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId!)
                    ??throw new EntityWithIDNotFoundException<Plan>(dto.PlanId!);

            }

            if (!dto.OrganizationTypeId.IsNullOrEmpty())
            {
                var organizationType = await _unitOfWork.OrganizationTypeRepository.FindAsync(dto.OrganizationTypeId!)
                    ?? throw new EntityWithIDNotFoundException<OrganizationType>(dto.OrganizationTypeId!);
            }


            //1.Add Owner
            var owner1 = _mapper.Map<Owner>(dto);

            var owner = new Owner
            {
                OwnerCode  = dto.OwnerCode,
                OwnerName = dto.OwnerName,
                OwnerIdCode = dto.OwnerIdCode,
                OwnerGender = dto.OwnerGender,
                OwnerDateOfBirth = dto.OwnerDateOfBirth,
                OwnerEthnic  = dto.OwnerEthnic,
                OwnerNational = dto.OwnerNational,
                OwnerAddress = dto.OwnerAddress,
                OwnerTaxCode = dto.OwnerTaxCode,
                OwnerType = dto.OwnerType,
                ProjectId = dto.ProjectId,
                PlanId = dto.PlanId,
                OwnerStatus = dto.OwnerStatus.ToString(),
                PublishedDate = dto.PublishedDate,
                PublishedPlace = dto.PublishedPlace,
                HusbandWifeName = dto.HusbandWifeName,
                RepresentPerson = dto.RepresentPerson,
                TaxPublishedDate = dto.TaxPublishedDate,
                OrganizationTypeId = dto.OrganizationTypeId,
            };


            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.OwnerRepository.AddAsync(owner);

            if (!dto.PlanId.IsNullOrEmpty())
            {
                var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId!);

                plan.TotalOwnerSupportCompensation += 1;

                plan.TotalPriceCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, null)
                                            + await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId);

                plan.TotalPriceLandSupportCompensation += await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId);

                plan.TotalPriceHouseSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.House);

                plan.TotalPriceArchitectureSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Architecture);

                plan.TotalPricePlantSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Plants);

                plan.TotalDeduction += await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(owner.OwnerId);

                plan.TotalLandRecoveryArea = plan.TotalLandRecoveryArea;

                //Tong Cong Chi phi den bu = (Tong Cong Gia Den Bu cua Owner - Deduction Owner)
                plan.TotalGpmbServiceCost += (decimal)((double)(plan.TotalPriceLandSupportCompensation + plan.TotalPriceHouseSupportCompensation
                                           + plan.TotalPriceArchitectureSupportCompensation + plan.TotalPricePlantSupportCompensation
                                           + plan.TotalOwnerSupportPrice) * 0.02);

            }

            //await _unitOfWork.CommitAsync();

            if (!dto.OwnerFiles.IsNullOrEmpty())
            {
                foreach (var file in owner.AttachFiles)
                {
                    file.OwnerId = owner.OwnerId;
                }
                await _attachFileService.CreateOwnerAttachFilesAsync(owner.OwnerId, dto.OwnerFiles);
            }

            var ownerReadDto = _mapper.Map<OwnerReadDTO>(owner);

            //2.Add Others Info
            //2.1 Add Supports
            if (!dto.OwnerSupports.IsNullOrEmpty())
            {
                
                ownerReadDto.Supports = await _supportService.CreateOwnerSupportsAsync(owner.OwnerId, dto.OwnerSupports!);
            }
            //2.2 Add Deductions
            if (!dto.OwnerDeductions.IsNullOrEmpty())
            {
                ownerReadDto.Deductions = await _deductionService.CreateOwnerDeductionsAsync(owner.OwnerId, dto.OwnerDeductions!);
            }
            //2.3 Gcn Land Info
            if (!dto.OwnerGcnlandInfos.IsNullOrEmpty())
            {
                ownerReadDto.GcnlandInfos = await _gcNLandInfoService.CreateOwnerGcnLandInfosAsync(owner.OwnerId, dto.OwnerGcnlandInfos!);
            }

            //2.4 AssetCompensations
            if (!dto.OwnerAssetCompensations.IsNullOrEmpty())
            {
                ownerReadDto.AssetCompensations = await _assetCompensationService.CreateOwnerAssetCompensationsAsync(owner.OwnerId, dto.OwnerAssetCompensations!);
            }

            return ownerReadDto;
        }

        public async Task<OwnerReadDTO> CreateOwnerWithFullInfomationAsync(OwnerWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId!, include: "ResettlementProject");

            if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId!);

            if (!dto.PlanId.IsNullOrEmpty())
            {
                var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId!)
                    ?? throw new EntityWithIDNotFoundException<Plan>(dto.PlanId!);
            }

            //this used to get widthdraw value from MeasuredLandinfo to caculate limitResettlementValue
            decimal ownerWidthdrawArea = 0;
            decimal ownerMeasuredPlotArea = 0;

            //1.Add Owner
            var owner1 = _mapper.Map<Owner>(dto);

            var owner = new Owner
            {
                OwnerCode = dto.OwnerCode,
                OwnerName = dto.OwnerName,
                OwnerIdCode = dto.OwnerIdCode,
                OwnerGender = dto.OwnerGender,
                OwnerDateOfBirth = dto.OwnerDateOfBirth,
                OwnerEthnic = dto.OwnerEthnic,
                OwnerNational = dto.OwnerNational,
                OwnerAddress = dto.OwnerAddress,
                OwnerTaxCode = dto.OwnerTaxCode,
                OwnerType = dto.OwnerType,
                ProjectId = dto.ProjectId,
                PlanId = dto.PlanId,
                OwnerStatus = dto.OwnerStatus.ToString(),
                PublishedDate = dto.PublishedDate,
                PublishedPlace = dto.PublishedPlace,
                HusbandWifeName = dto.HusbandWifeName,
                RepresentPerson = dto.RepresentPerson,
                TaxPublishedDate = dto.TaxPublishedDate,
                OrganizationTypeId = dto.OrganizationTypeId,
            };

            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.OwnerRepository.AddAsync(owner);
            //2.Add Owner Attach Files
            if (!dto.OwnerFiles.IsNullOrEmpty())
            {
                foreach (var file in dto.OwnerFiles!)
                {
                    var fileUpload = new UploadFileDTO
                    {
                        File = file.AttachFile!,
                        FileName = $"{file.Name}-{Guid.NewGuid()}",
                        FileType = FileTypeExtensions.ToFileMimeTypeString(file.FileType)
                    };

                    var attachFile = _mapper.Map<AttachFile>(file);

                    attachFile.OwnerId = owner.OwnerId;

                    attachFile.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

                    attachFile.CreatedBy = _userContextService.Username! ??
                        throw new CanNotAssignUserException();

                    await _unitOfWork.AttachFileRepository.AddAsync(attachFile);
                }
            }
            //3.Add Owner Support
            if (!dto.OwnerSupports.IsNullOrEmpty())
            {
                foreach (var item in dto.OwnerSupports!)
                {
                    if (!item.SupportTypeId.IsNullOrEmpty())
                    {
                        var supportType = await _unitOfWork.SupportTypeRepository.FindAsync(item.SupportTypeId!)
                        ?? throw new EntityWithIDNotFoundException<SupportType>(item.SupportTypeId!);
                    }

                    if (!item.AssetUnitId.IsNullOrEmpty())
                    {
                        var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(item.AssetUnitId!)
                        ?? throw new EntityWithIDNotFoundException<AssetUnit>(item.AssetUnitId!);
                    }

                    var support = _mapper.Map<Support>(item);

                    support.OwnerId = owner.OwnerId;

                    await _unitOfWork.SupportRepository.AddAsync(support);
                }

            }
            //4. Add Owner Deduction
            if (!dto.OwnerDeductions.IsNullOrEmpty())
            {
                foreach (var item in dto.OwnerDeductions!)
                {
                    var deduction = _mapper.Map<Deduction>(item);

                    deduction.OwnerId = owner.OwnerId;

                    await _unitOfWork.DeductionRepository.AddAsync(deduction);
                }
            }
            //5. Add Owner Gcn Land Info
            if (!dto.OwnerGcnlandInfos.IsNullOrEmpty())
            {
                foreach (var item in dto.OwnerGcnlandInfos!)
                {
                    
                    var landInfo = new GcnlandInfo()
                    {
                        GcnLandInfoId = Guid.NewGuid().ToString(),
                        GcnPageNumber = item.GcnPageNumber,
                        GcnPlotNumber = item.GcnPlotNumber,
                        GcnPlotAddress = item.GcnPlotAddress,
                        LandTypeId = item.LandTypeId,
                        GcnPlotArea = item.GcnPlotArea,
                        GcnOwnerCertificate = item.GcnOwnerCertificate,
                        //OwnerId = item.OwnerId,
                    };

                    landInfo.OwnerId = owner.OwnerId;

                    await _unitOfWork.GCNLandInfoRepository.AddAsync(landInfo);

                    foreach (var file in item.AttachFiles!)
                    {
                        var fileUpload = new UploadFileDTO
                        {
                            File = file.AttachFile!,
                            FileName = $"{file.Name}-{Guid.NewGuid()}",
                            FileType = FileTypeExtensions.ToFileMimeTypeString(file.FileType)
                        };

                        var attachFile = _mapper.Map<AttachFile>(file);

                        attachFile.GcnLandInfoId = landInfo.GcnLandInfoId;

                        attachFile.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

                        attachFile.CreatedBy = _userContextService.Username! ??
                            throw new CanNotAssignUserException();

                        await _unitOfWork.AttachFileRepository.AddAsync(attachFile);
                    }

                    if (!item.MeasuredLandInfos.IsNullOrEmpty())
                    {

                        foreach(var measuredLandDto in  item.MeasuredLandInfos!)
                        {
                        
                            var measuredLand = new MeasuredLandInfo
                            {

                                MeasuredPageNumber = measuredLandDto.MeasuredPageNumber,
                                MeasuredPlotNumber = measuredLandDto.MeasuredPlotNumber,
                                MeasuredPlotAddress = measuredLandDto.MeasuredPlotAddress,
                                LandTypeId = measuredLandDto.LandTypeId,
                                MeasuredPlotArea = measuredLandDto.MeasuredPlotArea,
                                WithdrawArea = measuredLandDto.WithdrawArea,
                                CompensationPrice = measuredLandDto.CompensationPrice,
                                CompensationRate = measuredLandDto.CompensationRate,
                                CompensationNote = measuredLandDto.CompensationNote,
                                UnitPriceLandCost = measuredLandDto.UnitPriceLandCost ?? 0,
                                OwnerId = owner.OwnerId,
                                UnitPriceLandId = measuredLandDto.UnitPriceLandId,
                                GcnLandInfoId = landInfo.GcnLandInfoId
                    
                            };

                            //measuredLand.OwnerId = owner.OwnerId;

                            ownerWidthdrawArea = measuredLand.WithdrawArea ?? 0;

                            ownerMeasuredPlotArea = measuredLand.MeasuredPlotArea ?? 0;

                            await _unitOfWork.MeasuredLandInfoRepository.AddAsync(measuredLand);

                            foreach (var file in measuredLandDto.AttachFiles!)
                            {
                                var fileUpload = new UploadFileDTO
                                {
                                    File = file.AttachFile!,
                                    FileName = $"{file.Name}-{Guid.NewGuid()}",
                                    FileType = FileTypeExtensions.ToFileMimeTypeString(file.FileType)
                                };

                                var attachFile = _mapper.Map<AttachFile>(file);

                                attachFile.MeasuredLandInfoId = measuredLand.MeasuredLandInfoId;

                                attachFile.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

                                attachFile.CreatedBy = _userContextService.Username! ??
                                    throw new CanNotAssignUserException();

                                await _unitOfWork.AttachFileRepository.AddAsync(attachFile);
                            }

                        }

                    }

                }
            }

            //6.Add Owner Land Resettlement
            //6.1 check if project support any resettlement
            if (project.ResettlementProject == null && !dto.OwnersLandResettlements.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot Create Owner Land Resettlement Because Project Does Not Support Any Resettlement");
            }

            if (!dto.OwnersLandResettlements.IsNullOrEmpty())
            {
                foreach(var landResettlementDto in dto.OwnersLandResettlements!)
                {
                    //find Resettlement Project associate with Land Resettlement
                    var associateResettlement = await _unitOfWork.ResettlementProjectRepository.FindAsync(landResettlementDto.ResettlementProjectId!)
                        ?? throw new EntityWithIDNotFoundException<ResettlementProject>(landResettlementDto.ResettlementProjectId!);

                    var limitResettlementValue = await CaculateLimitResettlementValueAsync(ownerWidthdrawArea, ownerMeasuredPlotArea);

                    // case2: if: result < limit_to_consideration <  limit_to_resettlement : khong dc tao Land Resettlement
                    //if (associateResettlement.LimitToResettlement > associateResettlement.LimitToConsideration && associateResettlement.LimitToConsideration > limitResettlementValue)
                    //{
                    //    throw new InvalidActionException($"Cannot Create Land Resettlement Because Owner Resettlement Limit Value: {limitResettlementValue}% < Project LimitToConsideration: {associateResettlement.LimitToConsideration}% < Project LimitToResettlement: {associateResettlement.LimitToResettlement}%.");
                    //}

                    //check if Project Resettlent still have enough land to resettlemt

                    var numberofResettlementOwner = await _unitOfWork.OwnerRepository.GetTotalLandResettlementsOfOwnersInProjectAsync(project.ProjectId);

                    if(project.ResettlementProject!.LandNumber <= numberofResettlementOwner + dto.OwnersLandResettlements.Count())
                    {
                        throw new InvalidActionException($"Không thể thêm đất tái định cư với số lượng: [{dto.OwnersLandResettlements.Count()}] cho chủ sở hữu: [{owner.OwnerCode}] vì đã vượt quá số lượng lô đất tái định cư: [{project.ResettlementProject.LandNumber}] có trong dự án: [{project.ProjectCode}].");
                    }

                    landResettlementDto.OwnerId = owner.OwnerId;

                    var landResettlement = _mapper.Map<LandResettlement>(landResettlementDto);
                    
                    await _unitOfWork.LandResettlementRepository.AddAsync(landResettlement);
                }

            }

            //7. Add Asset Compensation

            if (!dto.OwnerAssetCompensations.IsNullOrEmpty())
            {
                foreach(var item in dto.OwnerAssetCompensations!)
                {
                    var compensation = _mapper.Map<AssetCompensation>(item);

                    compensation.OwnerId = owner.OwnerId;

                    await _unitOfWork.AssetCompensationRepository.AddAsync(compensation);
                }

            }

            await _unitOfWork.CommitAsync();

            //8. Update Plan when add user
            if (!dto.PlanId.IsNullOrEmpty())
            {
                var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId!);

                plan.TotalOwnerSupportCompensation += 1;

                var serviceCostOfOwner = (decimal)((double)(await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId)
                  + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.House)
                  + await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId)
                  + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Architecture)
                  + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Plants))
                  * 0.02);


                //Boi Thuong Ho Tro Dat
                //TotalPriceOtherSupportCompensation = TotalPriceLandSupportCompensation
                plan.TotalPriceLandSupportCompensation += await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId);

                plan.TotalPriceHouseSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.House);

                plan.TotalPriceArchitectureSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Architecture);
                
                plan.TotalPricePlantSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Plants);

                plan.TotalDeduction += await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(owner.OwnerId);

                plan.TotalLandRecoveryArea += await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(owner.OwnerId);

                plan.TotalOwnerSupportPrice += _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId).Result;

                plan.TotalGpmbServiceCost += serviceCostOfOwner;

                plan.TotalPriceCompensation = plan.TotalPriceLandSupportCompensation
                + plan.TotalPriceHouseSupportCompensation
                + plan.TotalPriceArchitectureSupportCompensation
                + plan.TotalPricePlantSupportCompensation
                + plan.TotalOwnerSupportPrice
                + plan.TotalGpmbServiceCost;

            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }

        /// <summary>
        /// Caculate Limit Resettlement Value Of an Owner
        /// </summary>
        /// <param name="ownerWidthdrawArea"></param>
        /// <param name="ownerMeasuredPlotArea"></param>
        /// <returns></returns>
        private async Task<decimal> CaculateLimitResettlementValueAsync(decimal ownerWidthdrawArea, decimal ownerMeasuredPlotArea)
        {
            //devided by zero => return 0
            if(ownerMeasuredPlotArea == 0)
            {
                return 0;
            }
            return await Task.FromResult( (ownerWidthdrawArea / ownerMeasuredPlotArea) * 100);
        }
        public async Task<IEnumerable<OwnerWriteDTO>> ExtractOwnersFromFileAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length <= 0)
            {
                throw new InvalidActionException();
            }

            var importedUsers = new List<OwnerFileImportWriteDTO>();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                string[] parts = worksheet.Cells["D6"].Text.Split(':');

                var project = await _unitOfWork.ProjectRepository.GetProjectByProjectCodeAsync(parts[1].Trim());

                if (project == null) 
                    throw new EntityWithAttributeNotFoundException<Project>(nameof(Project.ProjectCode), parts[1].Trim());

                for (int row = 11; row <= worksheet.Dimension.End.Row; row++)
                {
                    
                    var ownerType = MapUsertypeEnumWithUserInput(worksheet.Cells[row, 13].Value?.ToString()!).ToString();

                    string organizationTypeId = "";

                    if (!ownerType.Contains("cánhân") || !ownerType.Contains("giađình"))
                    {
                        var organizationType = await _unitOfWork.OrganizationTypeRepository.FindByCodeAndIsDeletedStatus(worksheet.Cells[row, 16].Value?.ToString() ?? string.Empty, false)
                        ?? throw new EntityInputExcelException<OrganizationType>(nameof(Owner.OrganizationType), worksheet.Cells[row, 16].Value.ToString()!, row);
                        organizationTypeId = organizationType.OrganizationTypeId;
                    }

                    var user = new OwnerFileImportWriteDTO
                    {
                        OwnerCode = worksheet.Cells[row, 4].Value?.ToString() ?? string.Empty,
                        OwnerName = worksheet.Cells[row, 5].Value?.ToString() ?? string.Empty,
                        OwnerIdCode = worksheet.Cells[row, 6].Value?.ToString() ?? string.Empty,

                        PublishedDate = DateTime.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out DateTime publishDate)
                            ? publishDate
                            : null,

                        OwnerGender = worksheet.Cells[row, 8].Value?.ToString(),

                        OwnerDateOfBirth = DateTime.TryParse(worksheet.Cells[row, 9].Value?.ToString(), out DateTime dateOfBirth)
                            ? dateOfBirth
                            : null,

                        OwnerEthnic = worksheet.Cells[row, 10].Value?.ToString() ?? string.Empty,
                        OwnerNational = "Việt Nam",
                        OwnerAddress = worksheet.Cells[row, 12].Value?.ToString() ?? string.Empty,

                        
                        OwnerType = MapUsertypeEnumWithUserInput(worksheet.Cells[row, 13].Value?.ToString()!).ToString()
                            ?? throw new EntityInputExcelException<Owner>(nameof(Owner.OwnerType), worksheet.Cells[row, 13].ToString()!, row),


                        ProjectId = project.ProjectId,


                        //null start from here
                        HusbandWifeName = worksheet.Cells[row, 14].Value?.ToString() ?? string.Empty,

                        RepresentPerson = worksheet.Cells[row, 15].Value?.ToString() ?? string.Empty,

                        OrganizationTypeId = organizationTypeId,

                        OwnerTaxCode = worksheet.Cells[row, 17].Value?.ToString() ?? string.Empty,

                        TaxPublishedDate = DateTime.TryParse(worksheet.Cells[row, 18].Value?.ToString(), out DateTime taxPublishDate)
                            ? taxPublishDate
                            : null,
                        PublishedPlace = worksheet.Cells[row, 19].Value?.ToString() ?? string.Empty

                    };

                    importedUsers.Add(user);
                }
                package.Dispose();
            }

            await CheckDuplicateOwnersInImportFile(importedUsers);


            return _mapper.Map<IEnumerable<OwnerWriteDTO>>(importedUsers);
        }

        private async Task CheckDuplicateOwnersInImportFile(IEnumerable<OwnerFileImportWriteDTO> dtos)
        {
            var seenOwnerCodes = new Dictionary<string, int>();
            var seenOwnerIdCodes = new Dictionary<string, int>();
            var seenOwnerTaxCodes = new Dictionary<string, int>();


            for (int i = 0; i < dtos.Count(); i++)
            {
                var dto = dtos.ElementAt(i);

                if (!string.IsNullOrEmpty(dto.OwnerCode))
                {
                    if (seenOwnerCodes.TryGetValue(dto.OwnerCode, out var existingPosition))
                    {
                        throw new InvalidActionException($"Đã tìm thấy mục nhập trùng lặp của Mã chủ sở hữu '{dto.OwnerCode}' tại vị trí {existingPosition + 1} và {i + 1}");
                    }
                    seenOwnerCodes.Add(dto.OwnerCode, i);
                }

                if (!string.IsNullOrEmpty(dto.OwnerIdCode))
                {
                    if (seenOwnerIdCodes.TryGetValue(dto.OwnerIdCode, out var existingPosition))
                    {
                        throw new InvalidActionException($"Đã tìm thấy mục nhập trùng lặp của CCCD chủ sở hữu '{dto.OwnerIdCode}' tại vị trí {existingPosition + 1} và {i + 1}");
                    }
                    seenOwnerIdCodes.Add(dto.OwnerIdCode, i);
                }

                if (!string.IsNullOrEmpty(dto.OwnerTaxCode))
                {
                    if (seenOwnerTaxCodes.TryGetValue(dto.OwnerTaxCode, out var existingPosition))
                    {
                        throw new InvalidActionException($"Đã tìm thấy mục nhập trùng lặp của MST chủ sở hữu '{dto.OwnerTaxCode}' tại vị trí {existingPosition + 1} và {i + 1}");
                    }
                    seenOwnerTaxCodes.Add(dto.OwnerTaxCode, i);
                }
            }
        }


        private static ProjectOwnerTypeEnum MapUsertypeEnumWithUserInput(string name)
        {
            var input = name.ToLower().Replace(" ", "");
            if (input.Equals("cánhân")) return ProjectOwnerTypeEnum.Individual;
            if (input.Equals("giađình")) return ProjectOwnerTypeEnum.Household;
            if (input.Equals("tổchức")) return ProjectOwnerTypeEnum.Organization;
            return ProjectOwnerTypeEnum.Individual;
        }


        public async Task<IEnumerable<OwnerReadDTO>> ImportOwnerFromExcelFileAsync(IFormFile file)
        {

            var dtos = await ExtractOwnersFromFileAsync(file);

            var ownersList = new List<Owner>();

            foreach (var dto in dtos)
            {
                var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

                if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);


                if (!string.IsNullOrEmpty(dto.OwnerCode))
                {
                    var duplicateOwner = await _unitOfWork.OwnerRepository.FindByCodeAndIsDeletedStatus(dto.OwnerCode);

                    if (duplicateOwner != null)
                    {
                        throw new UniqueConstraintException($"Có một chủ sở hữu khác với Mã chủ sở hữu: [{dto.OwnerCode}] đã tồn tại trong hệ thống.");
                    }
                }
                if (!string.IsNullOrEmpty(dto.OwnerIdCode))
                {
                    var duplicateOwner = await _unitOfWork.OwnerRepository.FindByOwnerIdCodeInProjectAsync(dto.ProjectId,dto.OwnerIdCode);

                    if (duplicateOwner != null)
                    {
                        throw new UniqueConstraintException($"Có một chủ sở hữu khác với CCCD: [{dto.OwnerCode}] đã tồn tại trong hệ thống.");
                    }
                }
                if (!string.IsNullOrEmpty(dto.OwnerTaxCode))
                {
                    var duplicateOwner = await _unitOfWork.OwnerRepository.FindByTaxCodeInProjectAsync(dto.ProjectId,dto.OwnerTaxCode);

                    if (duplicateOwner != null)
                    {
                        throw new UniqueConstraintException($"Có một chủ sở hữu khác với MST: [{dto.OwnerTaxCode}] đã tồn tại trong hệ thống.");
                    }
                }
                var owner = _mapper.Map<Owner>(dto);


                owner.OwnerCreatedBy = _userContextService.Username!
                    ?? throw new CanNotAssignUserException();

                await _unitOfWork.OwnerRepository.AddAsync(owner);

                ownersList.Add(owner);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<OwnerReadDTO>>(ownersList);  
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

        /// <summary>
        /// Delete Owner then delete its GCN and Measured Land Info
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithIDNotFoundException{Owner}"></exception>
        /// <exception cref="InvalidActionException"></exception>
        /// <exception cref="EntityWithIDNotFoundException{MeasuredLandInfo}"></exception>
        public async Task DeleteOwner(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId, include: "GcnlandInfos");

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (owner.OwnerStatus == OwnerStatusEnum.AcceptCompensation.ToString()
                || owner.OwnerStatus == OwnerStatusEnum.RejectCompensation.ToString()
                || owner.PlanId != null)
                throw new InvalidActionException();

            if (owner.GcnlandInfos.Any())
            {
                foreach (var item in owner.GcnlandInfos)
                {
                    item.IsDeleted = true;
                }
            }

            var ownerMeasuredLandInfos = await _unitOfWork.MeasuredLandInfoRepository.GetAllMeasuredLandInfosOfOwnerAsync(ownerId);

            if (ownerMeasuredLandInfos.Any())
            {
                foreach (var item in ownerMeasuredLandInfos)
                {
                    var ownerMeasuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(item.MeasuredLandInfoId);
                    
                    if (ownerMeasuredLandInfo == null) throw new EntityWithIDNotFoundException<MeasuredLandInfo>(item.MeasuredLandInfoId);
                    
                    ownerMeasuredLandInfo.IsDeleted = true;
                }
            }

            owner.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }


        public async Task DeleteOldOwnerWhenCreatePlanCopy(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (owner.GcnlandInfos.Any())
            {
                foreach (var item in owner.GcnlandInfos)
                {
                    item.IsDeleted = true;
                }
            }

            var ownerMeasuredLandInfos = await _unitOfWork.MeasuredLandInfoRepository.GetAllMeasuredLandInfosOfOwnerAsync(ownerId);

            if (ownerMeasuredLandInfos.Any())
            {
                foreach (var item in ownerMeasuredLandInfos)
                {
                    var ownerMeasuredLandInfo = await _unitOfWork.MeasuredLandInfoRepository.FindAsync(item.MeasuredLandInfoId);

                    if (ownerMeasuredLandInfo == null) throw new EntityWithIDNotFoundException<MeasuredLandInfo>(item.MeasuredLandInfoId);

                    ownerMeasuredLandInfo.IsDeleted = true;
                }
            }

            owner.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }


        public async Task<OwnerReadDTO> GetOwnerAsync(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindIncludeIsActiveAsync(ownerId,
                include: "GcnlandInfos, GcnlandInfos.AttachFiles, GcnlandInfos.MeasuredLandInfos, GcnlandInfos.MeasuredLandInfos.LandType, GcnlandInfos.MeasuredLandInfos.AttachFiles," +
                " AssetCompensations, Supports, Deductions, Deductions.DeductionType, AttachFiles," +
                " LandResettlements, LandResettlements.ResettlementProject", trackChanges: false, isActive: false);
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

            if (!dto.PlanId.IsNullOrEmpty())
            {
                var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId!);

                if (plan == null) throw new EntityWithIDNotFoundException<Plan>(dto.PlanId!);
            }

            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            // Check if the Owner Code in the DTO is different from the Owner Code in the existing owner,
            // considering case-insensitive comparison and handling null or empty values.

            if(dto.OwnerCode!.ToLower() != owner.OwnerCode!.ToLower())
            {
                var duplicateOwner = await _unitOfWork.OwnerRepository.FindByCodeAndIsDeletedStatus(dto.OwnerCode);

                if (duplicateOwner != null)
                {
                    throw new UniqueConstraintException($"Có một chủ sở hữu khác với Mã chủ sở hữu: [{dto.OwnerCode}] đã tồn tại trong hệ thống.");
                }
            }
            if (dto.OwnerIdCode!.ToLower() != owner.OwnerIdCode!.ToLower())
            {
                var duplicateOwner = await _unitOfWork.OwnerRepository.FindByOwnerIdCodeAsync(dto.OwnerIdCode);

                if (duplicateOwner != null)
                {
                    throw new UniqueConstraintException($"Có một chủ sở hữu khác với CCCD: [{dto.OwnerCode}] đã tồn tại trong hệ thống.");
                }
            }
            if (dto.OwnerTaxCode!.ToLower() != owner.OwnerTaxCode!.ToLower())
            {
                var duplicateOwner = await _unitOfWork.OwnerRepository.FindByTaxCodeAsync(dto.OwnerTaxCode);

                if (duplicateOwner != null)
                {
                    throw new UniqueConstraintException($"Có một chủ sở hữu khác với MST: [{dto.OwnerTaxCode}] đã tồn tại trong hệ thống.");
                }
            }

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
        

        public async Task<IEnumerable<OwnerReadDTO>> AssignPlanToOwnerAsync(string planId, IEnumerable<string> ownerIds)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            var ownerList = new List<Owner>();

            foreach(var ownerId in ownerIds)
            {
                var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

                if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

                var serviceCostOfOwner = (decimal)((double)(await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId)
                   + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.House)
                   + await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId)
                   + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Architecture)
                   + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Plants))
                   * 0.02);

                owner.PlanId = planId;

                plan.TotalOwnerSupportCompensation += 1;

                plan.TotalPriceCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, null)
                                            + await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId);

                plan.TotalPriceLandSupportCompensation += await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId);

                plan.TotalPriceHouseSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.House);

                plan.TotalPriceArchitectureSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Architecture);

                plan.TotalPricePlantSupportCompensation += await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Plants);

                plan.TotalDeduction += await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(ownerId);

                plan.TotalLandRecoveryArea += await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(ownerId);

                plan.TotalOwnerSupportPrice += _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId).Result;

                plan.TotalGpmbServiceCost += serviceCostOfOwner;

                plan.TotalPriceCompensation = plan.TotalPriceLandSupportCompensation
                + plan.TotalPriceHouseSupportCompensation
                + plan.TotalPriceArchitectureSupportCompensation
                + plan.TotalPricePlantSupportCompensation
                + plan.TotalOwnerSupportPrice
                + plan.TotalGpmbServiceCost;

                ownerList.Add(owner);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<OwnerReadDTO>>(ownerList);
        }


        public async Task<IEnumerable<OwnerReadDTO>> RemoveOwnerFromPlanAsync(string planId, IEnumerable<string> ownerIds)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            var ownerList = new List<Owner>();

            if(!plan.PlanStatus.Contains(PlanStatusEnum.REJECTED.ToString()))
            {
                foreach (var ownerId in ownerIds)
                {
                    var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                    ?? throw new EntityWithIDNotFoundException<Owner>(ownerId);

                    if (owner.PlanId != planId)
                        throw new EntityWithAttributeNotFoundException<Owner>(nameof(Owner.PlanId), planId);

                    if (owner.OwnerStatus.Contains(OwnerStatusEnum.AcceptCompensation.ToString()) || owner.OwnerStatus.Contains(OwnerStatusEnum.RejectCompensation.ToString()))
                        throw new InvalidActionException();

                    owner.PlanId = null;

                    //Update Plan Price Info

                    plan.TotalOwnerSupportCompensation -= 1;

                    var serviceCostOfOwner = (decimal)((double)(await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId)
                        + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.House)
                        + await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId)
                        + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Architecture)
                        + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Plants))
                        * 0.02);

                    plan.TotalGpmbServiceCost -= serviceCostOfOwner;

                    plan.TotalPriceCompensation -= await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, null)
                        + await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId)
                        + await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId)
                        + serviceCostOfOwner;

                    plan.TotalPriceLandSupportCompensation -= await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId);

                    plan.TotalPriceHouseSupportCompensation -= await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.House);

                    plan.TotalPriceArchitectureSupportCompensation -= await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Architecture);

                    plan.TotalPricePlantSupportCompensation -= await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Plants);

                    plan.TotalDeduction -= await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(ownerId);

                    plan.TotalOwnerSupportPrice -= _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId).Result;

                    plan.TotalLandRecoveryArea -= await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(ownerId);

                    ownerList.Add(owner);
                }
            }
            else
            {
                foreach (var ownerId in ownerIds)
                {
                    var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                    ?? throw new EntityWithIDNotFoundException<Owner>(ownerId);

                    if (owner.PlanId != planId)
                        throw new EntityWithAttributeNotFoundException<Owner>(nameof(Owner.PlanId), planId);

                    if (owner.OwnerStatus.Contains(OwnerStatusEnum.AcceptCompensation.ToString()) || owner.OwnerStatus.Contains(OwnerStatusEnum.RejectCompensation.ToString()))
                        throw new InvalidActionException();

                    owner.PlanId = null;
                }
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<OwnerReadDTO>>(ownerList);
        }


        public async Task<OwnerReadDTO> RemoveOwnerFromProjectAsync(string ownerId, string projectId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (owner.ProjectId != projectId)
                throw new EntityWithAttributeNotFoundException<Owner>(nameof(Owner.ProjectId), projectId);

            if (owner.OwnerStatus.Contains(OwnerStatusEnum.AcceptCompensation.ToString()) || owner.OwnerStatus.Contains(OwnerStatusEnum.RejectCompensation.ToString()))
                throw new InvalidActionException();

            owner.PlanId = null;


            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="planId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithIDNotFoundException{Plan}"></exception>
        /// <exception cref="EntityWithIDNotFoundException{Project}"></exception>
        public async Task<PaginatedResponse<OwnerReadDTO>> GetOwnerInPlanByPlanIdAndOwnerInProjectThatNotInAnyPlanByProjectIdAsync(PaginatedQuery query, string planId, string projectId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId)
                ?? throw new EntityWithIDNotFoundException<Plan>(planId);
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId)
                ?? throw new EntityWithIDNotFoundException<Project>(projectId);

            var ownerInPlan = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId)
                ?? Enumerable.Empty<Owner>();

            var ownerNotInPlan = await _unitOfWork.OwnerRepository.GetOwnerInProjectThatNotInAnyPlanAsync(projectId)
                ?? Enumerable.Empty<Owner>();

            // Concatenate the collections
            var allOwners = ownerInPlan.Concat(ownerNotInPlan);

            return PaginatedResponse<OwnerReadDTO>.FromEnumerableWithMapping(allOwners, query, _mapper);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="ownerStatus"></param>
        /// <param name="rejectReason"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithIDNotFoundException{Owner}"></exception>
        /// <exception cref="CanNotAssignUserException"></exception>
        public async Task<OwnerReadDTO> UpdateOwnerStatusAsync(string ownerId, OwnerStatusEnum ownerStatus, string? rejectReason, AttachFileWriteDTO? file)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(ownerId);
            if (ownerStatus == OwnerStatusEnum.AcceptCompensation)
            {
                owner.OwnerStatus = OwnerStatusEnum.AcceptCompensation.ToString();

                if(file != null)
                {
                    var fileUpload = new UploadFileDTO
                    {
                        File = file.AttachFile!,
                        FileName = $"Chap-Nhan-Den-Bu-{file.Name}-{Guid.NewGuid()}",
                        FileType = FileTypeExtensions.ToFileMimeTypeString(file.FileType)
                    };

                    var attachFile = _mapper.Map<AttachFile>(file);

                    attachFile.OwnerId = owner.OwnerId;

                    attachFile.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

                    attachFile.CreatedBy = _userContextService.Username! ??
                        throw new CanNotAssignUserException();

                    await _unitOfWork.AttachFileRepository.AddAsync(attachFile);
                }
                
            }
            else if(ownerStatus == OwnerStatusEnum.RejectCompensation)
            {
                owner.OwnerStatus = OwnerStatusEnum.RejectCompensation.ToString();
                owner.RejectReason = rejectReason ?? "";
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }
        /// <summary>
        /// api check duplicate owner code
        /// </summary>
        /// <param name="ownerCode"></param>
        /// <returns></returns>
        /// <exception cref="UniqueConstraintException{Project}"></exception>
        public async Task<bool> CheckDuplicateOwnerCodeAsync(string ownerCode)
        {
            var owner = await _unitOfWork.OwnerRepository.FindByCodeAndIsDeletedStatus(ownerCode);
            if (owner != null)
            {
                throw new UniqueConstraintException<Project>(nameof(owner.OwnerCode), ownerCode);
            }
            return true;
        }

        /// <summary>
        /// api check duplicate owner id code in nproject
        /// </summary>
        /// <param name="ownerIdCode"></param>
        /// <returns></returns>
        /// <exception cref="UniqueConstraintException{Project}"></exception>
        public async Task<bool> CheckDuplicateOwnerIdCodeAsync(string projectId, string ownerIdCode)
        {
            var owner = await _unitOfWork.OwnerRepository.FindByOwnerIdCodeInProjectAsync(projectId, ownerIdCode);
            if (owner != null)
            {
                throw new UniqueConstraintException<Project>(nameof(owner.OwnerIdCode), ownerIdCode);
            }
            return true;
        }

        /// <summary>
        /// api check duplicate owner tax code in nproject
        /// </summary>
        /// <param name="ownerTaxCode"></param>
        /// <returns></returns>
        /// <exception cref="UniqueConstraintException{Project}"></exception>
        public async Task<bool> CheckDuplicateOwnerTaxCodeAsync(string projectId, string ownerTaxCode)
        {
            var owner = await _unitOfWork.OwnerRepository.FindByTaxCodeInProjectAsync(projectId, ownerTaxCode);
            if (owner != null)
            {
                throw new UniqueConstraintException<Project>(nameof(owner.OwnerTaxCode), ownerTaxCode);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PaginatedResponse<OwnerReadDTO>> QueryOwnersOfProjectAsync(string projectId, OwnerQuery query)
        {
            var owner = await _unitOfWork.OwnerRepository.QueryOwnersOfProjectAsync(projectId, query);
            return PaginatedResponse<OwnerReadDTO>.FromEnumerableWithMapping(owner, query, _mapper);
        }
        /// <summary>
        /// Get Owners Have Land Resettlement In Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OwnerReadDTO>> GetOwnersHaveLandResettlementInProjectAsync(string projectId)
        {
            return _mapper.Map<IEnumerable<OwnerReadDTO>>(await _unitOfWork.OwnerRepository.GetOwnersHaveLandResettlementInProjectAsync(projectId));
        }

    }

}
