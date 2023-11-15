using Amazon.S3.Model;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Implementations;
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
            var owner = _mapper.Map<Owner>(dto);

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
                plan.TotalGpmbServiceCost += plan.TotalPriceCompensation - plan.TotalDeduction;

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
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId!);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId!);

            if (!dto.PlanId.IsNullOrEmpty())
            {
                var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId!)
                    ?? throw new EntityWithIDNotFoundException<Plan>(dto.PlanId!);
            }

            //1.Add Owner
            var owner = _mapper.Map<Owner>(dto);

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
                            measuredLandDto.OwnerId = owner.OwnerId;

                            var measuredLand = _mapper.Map<MeasuredLandInfo>(measuredLandDto);

                            //measuredLand.OwnerId = owner.OwnerId;

                            await _unitOfWork.MeasuredLandInfoRepository.AddAsync(measuredLand);

                            foreach (var file in item.AttachFiles!)
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

            await _unitOfWork.CommitAsync();

            //6. Update Plan when add user
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

                plan.TotalLandRecoveryArea += await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(owner.OwnerId);

                //Tong Cong Chi phi den bu = (Tong Cong Gia Den Bu cua Owner - Deduction Owner)
                plan.TotalGpmbServiceCost += plan.TotalPriceCompensation - plan.TotalDeduction;

            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
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

                var project = await _unitOfWork.ProjectRepository.GetProjectByNameAsync(parts[1].Trim());

                if (project == null) 
                    throw new EntityWithAttributeNotFoundException<Project>(nameof(Project.ProjectName), parts[1].Trim());

                for (int row = 11; row <= worksheet.Dimension.End.Row; row++)
                {
                    var organizationType = await _unitOfWork.OrganizationTypeRepository.FindByCodeAndIsDeletedStatus(worksheet.Cells[row, 16].Value?.ToString() ?? string.Empty, false)
                        ?? throw new EntityInputExcelException<Owner>(nameof(Owner.OrganizationType), worksheet.Cells[row, 16].Value.ToString()!, row);

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
                        OwnerNational = worksheet.Cells[row, 11].Value?.ToString() ?? string.Empty,
                        OwnerAddress = worksheet.Cells[row, 12].Value?.ToString() ?? string.Empty,

                        OwnerType = MapUsertypeEnumWithUserInput(worksheet.Cells[row, 13].Value?.ToString()!).ToString()
                            ?? throw new EntityInputExcelException<Owner>(nameof(Owner.OwnerType), worksheet.Cells[row, 13].Value.ToString()!, row),


                        ProjectId = project.ProjectId,


                        //null start from here
                        HusbandWifeName = worksheet.Cells[row, 14].Value?.ToString() ?? string.Empty,

                        RepresentPerson = worksheet.Cells[row, 15].Value?.ToString() ?? string.Empty,

                        
                        OrganizationTypeId = organizationType.OrganizationTypeId,

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
            return _mapper.Map<IEnumerable<OwnerWriteDTO>>(importedUsers);
        }

        private static ProjectOwnerTypeEnum MapUsertypeEnumWithUserInput(string name)
        {
            var input = name.ToLower().Split(" ");
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

                //var plan = await _unitOfWork.PlanRepository.FindAsync(dto.PlanId);

                //if (plan == null) throw new EntityWithIDNotFoundException<Plan>(dto.PlanId);

                var owner = _mapper.Map<Owner>(dto);

                owner.OwnerCreatedBy = _userContextService.Username!
                    ?? throw new CanNotAssignUserException();

                await _unitOfWork.OwnerRepository.AddAsync(owner);

                await _unitOfWork.CommitAsync();

                ownersList.Add(owner);
            }

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

        public async Task DeleteOwner(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (owner.OwnerStatus == OwnerStatusEnum.AcceptCompensation.ToString()
                || owner.OwnerStatus == OwnerStatusEnum.RejectCompensation.ToString()
                || owner.PlanId != null)
                throw new InvalidActionException();

            owner.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<OwnerReadDTO> GetOwnerAsync(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId, include: "GcnlandInfos, GcnlandInfos.MeasuredLandInfos, GcnlandInfos.MeasuredLandInfos.AttachFiles, AssetCompensations, Supports, Deductions");
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

                ownerList.Add(owner);
            }

            //Tong Cong Chi phi den bu = (Tong Cong Gia Den Bu cua Owner - Deduction Owner)
            plan.TotalGpmbServiceCost += plan.TotalPriceCompensation - plan.TotalDeduction;

            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<OwnerReadDTO>>(ownerList);
        }


        public async Task<OwnerReadDTO> RemoveOwnerFromPlanAsync(string ownerId, string planId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (owner.PlanId != planId) 
                throw new EntityWithAttributeNotFoundException<Owner>(nameof(Owner.PlanId), planId);

            if(owner.OwnerStatus == OwnerStatusEnum.AcceptCompensation.ToString() || owner.OwnerStatus == OwnerStatusEnum.RejectCompensation.ToString())
                throw new InvalidActionException();

            owner.PlanId = null;

            //Update Plan Price Info
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId)
                ?? throw new EntityWithIDNotFoundException<Plan>(planId);

            plan.TotalOwnerSupportCompensation -= 1;

            //Tong Cong Gia Den Bu =  (Dat + Tai San) cua Owner
            plan.TotalPriceCompensation -= await  _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, null) 
                + await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId);

            plan.TotalPriceLandSupportCompensation -=  await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(ownerId);

            plan.TotalPriceHouseSupportCompensation -= await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.House);

            plan.TotalPriceArchitectureSupportCompensation -= await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Architecture);

            plan.TotalPricePlantSupportCompensation -=  await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(ownerId, AssetOnLandTypeEnum.Plants);

            plan.TotalDeduction -= await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(ownerId);

            plan.TotalLandRecoveryArea -= await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(ownerId);

            //Tong Cong Chi phi den bu = (Tong Cong Gia Den Bu cua Owner - Deduction Owner)
            plan.TotalGpmbServiceCost -= plan.TotalPriceCompensation - plan.TotalDeduction; 

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }


        public async Task<OwnerReadDTO> RemoveOwnerFromProjectAsync(string ownerId, string projectId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                ?? throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (owner.ProjectId != projectId)
                throw new EntityWithAttributeNotFoundException<Owner>(nameof(Owner.ProjectId), projectId);

            if (owner.OwnerStatus == OwnerStatusEnum.AcceptCompensation.ToString() || owner.OwnerStatus == OwnerStatusEnum.RejectCompensation.ToString())
                throw new InvalidActionException();

            owner.PlanId = null;

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }

        public async Task<PaginatedResponse<OwnerReadDTO>> GetOwnerInPlanByPlanIdAndOwnerInProjectThatNotInAnyPlanByProjectIdAsync(PaginatedQuery query, string planId, string projectId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId)
                ?? throw new EntityWithIDNotFoundException<Plan>(planId);
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId)
                ?? throw new EntityWithIDNotFoundException<Project>(projectId);

            var ownerInPlan = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId);

            var ownerNotInPlan = await _unitOfWork.OwnerRepository.GetOwnerInProjectThatNotInAnyPlanAsync(projectId);

            ownerInPlan.Concat(ownerNotInPlan);

            return PaginatedResponse<OwnerReadDTO>.FromEnumerableWithMapping(ownerInPlan, query, _mapper);

        }



    }
}
