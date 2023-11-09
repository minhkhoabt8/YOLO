using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Enums;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Globalization;
using System.Reflection;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAttachFileService _attachFileService;
        private readonly IAuthService _authService;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IAttachFileService attachFileService, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _attachFileService = attachFileService;
            _authService = authService;
        }

        public async Task<PlanReadDTO> CreatePlanAsync(PlanWriteDTO dto)
        {
            var existProject = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if(existProject == null)
            {
                throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);
            }

            if (!dto.PlanApprovedBy.IsNullOrEmpty())
            {
                var signer = await _authService.GetAccountByIdAsync(dto.PlanApprovedBy!);

                if (signer == null || signer.Role.Id != ((int)AuthRoleEnum.Approval).ToString())
                {
                    throw new CannotAssignSignerException();
                }

                dto.PlanApprovedBy = signer.Id;
            }

            var plan = _mapper.Map<Plan>(dto);

            plan.PlanCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.PlanRepository.AddAsync(plan);

            if (!dto.AttachFiles.IsNullOrEmpty())
            {
                foreach (var item in dto.AttachFiles!)
                {
                    item.PlanId = plan.PlanId;
                }

                await _attachFileService.UploadAttachFileAsync(dto.AttachFiles!);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(plan);
        }

        public async Task DeletePlan(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            plan.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<PlanReadDTO> GetPlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);
            return _mapper.Map<PlanReadDTO>(plan);
        }

        public Task ImportPlan(IFormFile attachFile)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResponse<PlanReadDTO>> QueryPlanAsync(PlanQuery query)
        {
            var plan = await _unitOfWork.PlanRepository.QueryAsync(query);
            return PaginatedResponse<PlanReadDTO>.FromEnumerableWithMapping(plan, query, _mapper);
        }

        public async Task<PlanReadDTO> UpdatePlanAsync(string planId, PlanWriteDTO dto)
        {
            var existProject = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (existProject == null)
            {
                throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);
            }

            var existApprover = await _unitOfWork.OwnerRepository.FindAsync(dto.PlanApprovedBy);

            if (existApprover == null)
            {
                throw new EntityWithIDNotFoundException<Owner>(dto.PlanApprovedBy);
            }

            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Owner>(planId);

            _mapper.Map(dto, plan);

            plan.PlanCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(plan);
        }

        /// <summary>
        /// This method only used to test export all plans in database
        /// </summary>
        /// <returns></returns>
        public async Task<ExportFileDTO> ExportPlansFileAsync(string projectId)
        {
            var plans = await _unitOfWork.PlanRepository.GetPlansOfProjectAsync(projectId);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Plans");

                int row = 1;

                var properties = typeof(Plan).GetProperties();

                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = properties[col].Name;
                }

                foreach (var item in plans)
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
        /// Tao Boi Thuong Ho Tro File Doc
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<ExportFileDTO> ExportBTHTPlansWordAsync(string planId)
        {
            //Get Data BTHT
            var dataBTHT = await GetDataForBTHTPlanAsync(planId)
                ?? throw new Exception("Value is null");

            //Get File Template
            var fileName = GetFileTemplateDirectory.Get("PhuongAn_BaoCao");

            //Create new File Based on Template
            var fileDest = Path.Combine(Directory.GetCurrentDirectory(),"Temp"
                , $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(fileName)}");
            
            if (!CopyTemplate(fileName, fileDest)) throw new Exception("Cannot Create File");

            //Fill in data
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileDest, true, new OpenSettings { AutoSave = false }))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();
                foreach (var para in paras)
                {
                    foreach (var run in para.Elements<Run>())
                    {
                        foreach (var text in run.Elements<Text>())
                        {
                            Console.Write(text.Text + "\n");
                            if (text.Text.Contains("tenduan"))
                                text.Text = text.Text.Replace("tenduan", dataBTHT.ProjectName);
                            if (text.Text.Contains("cancu"))
                                text.Text = text.Text.Replace("cancu", dataBTHT.PlanBasedOn == null ? "" : dataBTHT.PlanBasedOn);
                            if (text.Text.Contains("diadiem"))
                                text.Text = text.Text.Replace("diadiem", dataBTHT.PlanLocation);
                            if (text.Text.Contains("Sumdientichthuhoi"))
                                text.Text = text.Text.Replace("Sumdientichthuhoi", string.Format("{0:#,###.## m2}", dataBTHT.TotalLandRecoveryArea));
                            if (text.Text.Contains("countvanban"))
                                text.Text = text.Text.Replace("countvanban", string.Format("{0:#,##0}", dataBTHT.TotalOwnerSupportCompensation));
                            if (text.Text.Contains("diachithuhoi"))
                                text.Text = text.Text.Replace("diachithuhoi", dataBTHT.LandAcquisitionAddress);
                            if (text.Text.Contains("boithuongdat"))
                                text.Text = text.Text.Replace("boithuongdat", string.Format("{0:#,##0đ}", dataBTHT.TotalPriceLandSupportCompensation));
                            if (text.Text.Contains("boithuongnha"))
                                text.Text = text.Text.Replace("boithuongnha", string.Format("{0:#,##0đ}", dataBTHT.TotalPriceHouseSupportCompensation));
                            if (text.Text.Contains("boithuongvat"))
                                text.Text = text.Text.Replace("boithuongvat", string.Format("{0:#,##0đ}", dataBTHT.TotalPriceArchitectureSupportCompensation));
                            if (text.Text.Contains("boithuongcay"))
                                text.Text = text.Text.Replace("boithuongcay", string.Format("{0:#,##0đ}", dataBTHT.TotalPricePlantSupportCompensation));
                            if (text.Text.Contains("boithuongkhac"))
                                text.Text = text.Text.Replace("boithuongkhac", string.Format("{0:#,##0đ}", dataBTHT.TotalPriceOtherSupportCompensation));
                            if (text.Text.Contains("chiphiphucvu"))
                                text.Text = text.Text.Replace("chiphiphucvu", string.Format("{0:#,##0đ}", dataBTHT.TotalGpmbServiceCost));
                        }

                    }
                }
                wordDoc.Save();
                var mainPart = wordDoc.MainDocumentPart;
                wordDoc.Close();
                wordDoc.Dispose();
            }
            byte[] fileBytes = File.ReadAllBytes(fileDest);

            File.Delete(fileDest);

            return new ExportFileDTO
            {
                FileName = Path.GetFileName(fileDest),
                FileByte = fileBytes,
                FileType = FileTypeExtensions.ToFileMimeTypeString(FileTypeEnum.docx) // Change this to the appropriate content type for Word documents
            };

            
        }

        /// <summary>
        /// Lay Data Tu DB len cho BTHT Export File
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithIDNotFoundException{Plan}"></exception>
        /// <exception cref="EntityWithAttributeNotFoundException{Project}"></exception>
        private async Task<BTHTPlanReadDTO> GetDataForBTHTPlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId)
                ?? throw new EntityWithIDNotFoundException<Plan>(planId);

            var project = await _unitOfWork.ProjectRepository.GetProjectByPlandIdAsync(planId)
                ?? throw new EntityWithAttributeNotFoundException<Project>(nameof(Plan.PlanId), planId);

            return new BTHTPlanReadDTO
            {
                ProjectName = project.ProjectName,
                ProjectLocation = project.ProjectLocation,

                PlanName = project.ProjectName, //need change
                PlanLocation = project.ProjectLocation,
                PlanBasedOn = plan.PlanCreateBase,

                TotalLandRecoveryArea = 0, //need updated
                TotalOwnerSupportCompensation = plan.TotalOwnerSupportCompensation,
                LandAcquisitionAddress = plan.Project.ProjectLocation, //the same as PlanLocation value
                TotalPriceLandSupportCompensation = plan.TotalPriceLandSupportCompensation,
                TotalPriceHouseSupportCompensation = plan.TotalPriceHouseSupportCompensation,
                TotalPriceArchitectureSupportCompensation = plan.TotalPriceArchitectureSupportCompensation,
                TotalPricePlantSupportCompensation = plan.TotalPricePlantSupportCompensation,
                TotalPriceOtherSupportCompensation = 0,
                TotalGpmbServiceCost = plan.TotalGpmbServiceCost// needd updated

            };
        }

        /// <summary>
        /// Create Temp File To Export
        /// </summary>
        /// <param name="fileSource"></param>
        /// <param name="fileDest"></param>
        /// <returns></returns>
        private bool CopyTemplate(string fileSource, string fileDest)
        {
            try
            {
                File.Copy(fileSource, fileDest);

                using (var wordDocument = WordprocessingDocument.Open(fileDest, true))
                {
                    var mainPart = wordDocument.MainDocumentPart;
                    if (mainPart != null)
                    {
                        // Define content
                        var text = new Text("Hello Open XML world");
                        var run = new Run(text);
                        var paragraph = new Paragraph(run);

                        // Add the paragraph to the document body
                        mainPart.Document.Body.AppendChild(paragraph);

                        // Save the changes
                        mainPart.Document.Save();
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Use this method to check and reassign prices value of plan when price settings or owners of plan were changed
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        // TODO: Need Finish
        public async Task ReCheckPricesOfPlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);
            if(plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);
            //1.Caculate all owner of a plan with status isDelete = false, => Sum total_owner_support_compensation
            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId);
            plan.TotalOwnerSupportCompensation = owners.Count();
            //2.For each owner, re-caculating related prices and reassign it to plan
        }

        /// <summary>
        /// Bảng Tổng Hợp Thu Hồi Report Excel 
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public Task<ExportFileDTO> ExportSummaryOfRecoveryExcelAsync(string planId)
        {
            //Get Data From DB
            throw new NotImplementedException();
        }


        private async Task<SummaryOfRecoveryReadDTO> GetDataForSummaryOfRecoveryAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId)
                ?? throw new EntityWithIDNotFoundException<Plan>(planId);

            var project = await _unitOfWork.ProjectRepository.GetProjectByPlandIdAsync(planId)
                ?? throw new EntityWithAttributeNotFoundException<Project>(nameof(Plan.PlanId), planId); ;

            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId)
                ?? throw new EntityWithAttributeNotFoundException<Owner>(nameof(Plan.PlanId), planId);

            throw new NotImplementedException();
        }


        public async Task<IEnumerable<PlanReadDTO>> GetPlansOfProjectASync(string projectId)
        {
            return _mapper.Map<IEnumerable<PlanReadDTO>>(await _unitOfWork.PlanRepository.GetPlansOfProjectAsync(projectId));
        }
    }
}
