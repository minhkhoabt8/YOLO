using Amazon.S3.Model;
using AutoMapper;
using BitMiracle.LibTiff.Classic;
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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Enums;
using SharedLib.Core.Exceptions;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Globalization;
using System.IO.Packaging;
using System.Numerics;
using System.Reflection;
using System.Text;
using Xceed.Words.NET;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAttachFileService _attachFileService;
        private readonly IAuthService _authService;
        private readonly IOwnerService _ownerService;
        private readonly IGetFileTemplateDirectory _getFileTemplateDirectory;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IAttachFileService attachFileService, IAuthService authService, IOwnerService ownerService, IGetFileTemplateDirectory getFileTemplateDirectory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _attachFileService = attachFileService;
            _authService = authService;
            _ownerService = ownerService;
            _getFileTemplateDirectory = getFileTemplateDirectory;
        }

        public async Task<PlanReadDTO> CreatePlanAsync(PlanWriteDTO dto)
        {
            var existProject = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (existProject == null)
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

            //valid check duplicate plan code
            var plancode = await _unitOfWork.PlanRepository.GetPlanByPlanCodeAsync(plan.PlanCode);
            if (plancode != null)
            {
                throw new UniqueConstraintException<Plan>(nameof(plan.PlanCode), plancode.PlanCode);
            }
            ///

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

            if (plan.PlanStatus == PlanStatusEnum.REJECTED.ToString()
                || plan.PlanStatus == PlanStatusEnum.AWAITING.ToString()
                || plan.PlanStatus == PlanStatusEnum.APPROVED.ToString())
            {
                throw new InvalidActionException($"Cannot Delete Plan With Status [{plan.PlanStatus}].");
            }

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
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Core.Entities.Owner>(planId);

            if (plan.PlanStatus == PlanStatusEnum.REJECTED.ToString()
                || plan.PlanStatus == PlanStatusEnum.AWAITING.ToString()
                || plan.PlanStatus == PlanStatusEnum.APPROVED.ToString())
            {
                throw new InvalidActionException($"Cannot Update Plan With Status [{plan.PlanStatus}].");
            }

            var existProject = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId);

            if (existProject == null)
            {
                throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);
            }


            var existApprover = await _authService.GetAccountByIdAsync(dto.PlanApprovedBy);

            if (existApprover == null)
            {
                throw new EntityWithIDNotFoundException<Core.Entities.Owner>(dto.PlanApprovedBy);
            }

            if (existApprover.Role.Name != AuthRoleEnum.Approval.ToString()) throw new CannotAssignSignerException();

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



        public async Task<byte[]> ConvertDocxToPdf(string docxFilePath)
        {

            DocxToPdfRenderer renderer = new DocxToPdfRenderer();

            PdfDocument pdf = renderer.RenderDocxAsPdf(docxFilePath);

            using (var memoryStream = new MemoryStream())
            {
                pdf.Stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private string GetParagraphStyle(Xceed.Document.NET.Paragraph paragraph)
        {
            // You may customize this method based on the paragraph styles you want to include
            // For example, you can access paragraph properties like paragraph.Bold, paragraph.Italic, etc.
            // and convert them to corresponding HTML styles.
            // This is just a placeholder, and you should adjust it based on your needs
            return "font-family: Arial; font-size: 12pt;";
        }

        /// <summary>
        /// Lay Data Tu DB len cho Phuong An Bao Cao Export File
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

                TotalLandRecoveryArea = plan.TotalLandRecoveryArea, //need updated
                TotalOwnerSupportCompensation = plan.TotalOwnerSupportCompensation,
                LandAcquisitionAddress = plan.Project.ProjectLocation, //the same as PlanLocation value
                TotalPriceLandSupportCompensation = plan.TotalPriceLandSupportCompensation,
                TotalPriceHouseSupportCompensation = plan.TotalPriceHouseSupportCompensation,
                TotalPriceArchitectureSupportCompensation = plan.TotalPriceArchitectureSupportCompensation,
                TotalPricePlantSupportCompensation = plan.TotalPricePlantSupportCompensation,
                TotalPriceOtherSupportCompensation = plan.TotalPriceLandSupportCompensation,
                TotalGpmbServiceCost = plan.TotalGpmbServiceCost// needd updated

            };
        }



        public async Task<List<DetailBTHChiPhiReadDTO>> getDataForBTHChiPhiAsync(string planId)
        {
            List<DetailBTHChiPhiReadDTO> details = new List<DetailBTHChiPhiReadDTO>();

            var plan = await _unitOfWork.PlanRepository.FindAsync(planId)
                ?? throw new EntityWithIDNotFoundException<Plan>(planId);

            var project = await _unitOfWork.ProjectRepository.GetProjectByPlandIdAsync(planId)
                ?? throw new EntityWithAttributeNotFoundException<Project>(nameof(Plan.PlanId), planId);

            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId)
                ?? throw new EntityWithAttributeNotFoundException<Core.Entities.Owner>(nameof(Plan.PlanId), planId);
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetUnitPriceCodeByProjectAsync(project.PriceAppliedCodeId)
                ?? throw new EntityWithAttributeNotFoundException<PriceAppliedCode>(nameof(Project.PriceAppliedCodeId), project.PriceAppliedCodeId);

            foreach (var item in owners)
            {
                var measuredLandInfos = await _unitOfWork.MeasuredLandInfoRepository.GetAllMeasuredLandInfosOfOwnerAsync(item.OwnerId)
                ?? throw new EntityWithAttributeNotFoundException<MeasuredLandInfo>(nameof(Core.Entities.Owner.OwnerId), item.OwnerId);
                int stt = 0;
                foreach (var i in measuredLandInfos)
                {
                    DetailBTHChiPhiReadDTO detail = new DetailBTHChiPhiReadDTO();
                    detail.Stt = stt + 1;
                    detail.ProjectCode = project.ProjectCode;
                    detail.OwnerCode = item.OwnerCode;
                    detail.OwnerName = item.OwnerName;
                    detail.Province = project.Province;
                    detail.District = project.District;
                    detail.Ward = project.Ward;
                    detail.ProjectName = project.ProjectName;
                    //
                    detail.UnitPriceCode = priceAppliedCode.UnitPriceCode;
                    //
                    detail.MeasuredPageNumber = i.MeasuredPageNumber;
                    detail.MeasuredPlotNumber = i.MeasuredPlotNumber;
                    detail.MeasuredPlotArea = i.MeasuredPlotArea.ToString();
                    detail.WithdrawArea = i.WithdrawArea;
                    var laneType = await _unitOfWork.LandTypeRepository.GetLandTypesOfMeasureLandInfoAsync(i.LandTypeId);
                    detail.CodeLandType = laneType.Code;
                    detail.SumLandCompensation = await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(item.OwnerId, true);
                    detail.SumHouseCompensationPrice = await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(item.OwnerId, AssetOnLandTypeEnum.House);
                    detail.SumTreeCompensationPrice = await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(item.OwnerId, AssetOnLandTypeEnum.Plants);
                    detail.SumArchitectureCompensationPrice = await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(item.OwnerId, AssetOnLandTypeEnum.Architecture);
                    detail.SumSupportPrice = await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(item.OwnerId);
                    detail.SumDeductionPrice = await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(item.OwnerId);
                    detail.SumBTHT = (decimal)(detail.SumLandCompensation + detail.SumHouseCompensationPrice + detail.SumTreeCompensationPrice + detail.SumArchitectureCompensationPrice + detail.SumSupportPrice - detail.SumDeductionPrice);
                    details.Add(detail);
                }
            }
            return details;
        }


        /// <summary>
        /// export Bảng Tổng Hợp Chi Phí 
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExportFileDTO> ExportBTHChiPhiToExcelAsync(string planId, FileTypeEnum filetype = FileTypeEnum.xlsx)
        {
            var detail = await getDataForBTHChiPhiAsync(planId) ?? throw new Exception("Value is null");

            var templateFileName = _getFileTemplateDirectory.Get("BangTongHopChiPhiBT");
            var templateFile = new FileInfo(templateFileName);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (!templateFile.Exists)
            {
                throw new Exception("File not found");
            }


            var tempFile = new FileInfo(Path.GetTempFileName());

            byte[] fileBytes;
            using (var package = new ExcelPackage(templateFile))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int row = 11;
                int sttCounter = 1;
                foreach (var item in detail)
                {
                    string maVanBan = $"{item.OwnerCode} / {item.UnitPriceCode} - {item.ProjectCode}";
                    string diaDiemThuHoiDat = $"{item.Ward} / {item.District} /{item.Province} ";
                    for (int col = 1; col <= 20; col++)
                    {
                        worksheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }


                    worksheet.Cells[row, 1].Value = sttCounter;
                    worksheet.Cells[row, 2].Value = maVanBan;
                    worksheet.Cells[row, 3].Value = item.OwnerName;
                    worksheet.Cells[row, 4].Value = diaDiemThuHoiDat;
                    worksheet.Cells[row, 5].Value = item.MeasuredPageNumber;
                    worksheet.Cells[row, 6].Value = item.MeasuredPlotNumber;
                    worksheet.Cells[row, 7].Value = item.CodeLandType;
                    worksheet.Cells[row, 8].Value = item.MeasuredPlotArea;
                    /*worksheet.Cells[row, 9].Value = item.MeasuredPageNumber;*/
                    worksheet.Cells[row, 10].Value = item.WithdrawArea;
                    worksheet.Cells[row, 11].Value = item.SumLandCompensation;
                    worksheet.Cells[row, 12].Value = item.SumHouseCompensationPrice;
                    worksheet.Cells[row, 13].Value = item.SumArchitectureCompensationPrice;
                    worksheet.Cells[row, 14].Value = item.SumTreeCompensationPrice;
                    worksheet.Cells[row, 15].Value = item.SumSupportPrice;
                    worksheet.Cells[row, 16].Value = item.SumDeductionPrice;
                    worksheet.Cells[row, 17].Value = item.SumBTHT;

                    worksheet.Cells[6, 4].Value = "Dự án: " + item.ProjectName;

                    row++;
                    sttCounter++;
                }

                // Save to the temporary file
                package.SaveAs(tempFile);
            }


            fileBytes = File.ReadAllBytes(tempFile.FullName);


            File.Delete(tempFile.FullName);


            var exportFileName = $"{Path.GetFileNameWithoutExtension(templateFileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(templateFileName)}";

            return new ExportFileDTO
            {
                FileName = exportFileName,
                FileByte = fileBytes,
                FileType = FileTypeExtensions.ToFileMimeTypeString(filetype)
            };
        }



        /// <summary>
        /// export Bảng Tổng Hợp Thu Hồi
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExportFileDTO> ExportBTHThuHoiToExcelAsync(string planId, FileTypeEnum filetype = FileTypeEnum.xlsx)
        {
            var detail = await getDataForBTHChiPhiAsync(planId) ?? throw new Exception("Value is null");

            var templateFileName = _getFileTemplateDirectory.Get("BangTongHopThuHoi");
            var templateFile = new FileInfo(templateFileName);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (!templateFile.Exists)
            {
                throw new Exception("File not found");
            }


            var tempFile = new FileInfo(Path.GetTempFileName());

            byte[] fileBytes;
            using (var package = new ExcelPackage(templateFile))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int row = 10;
                int sttCounter = 1;
                foreach (var item in detail)
                {
                    string maVanBan = $"{item.OwnerCode} / {item.UnitPriceCode} - {item.ProjectCode}";
                    string diaDiemThuHoiDat = $"{item.Ward} / {item.District} /{item.Province} ";
                    for (int col = 1; col <= 20; col++)
                    {
                        worksheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    }


                    worksheet.Cells[row, 1].Value = sttCounter;
                    worksheet.Cells[row, 2].Value = maVanBan;
                    worksheet.Cells[row, 3].Value = item.OwnerName;
                    worksheet.Cells[row, 4].Value = diaDiemThuHoiDat;
                    worksheet.Cells[row, 5].Value = item.MeasuredPageNumber;
                    worksheet.Cells[row, 6].Value = item.MeasuredPlotNumber;
                    worksheet.Cells[row, 7].Value = item.CodeLandType;
                    worksheet.Cells[row, 8].Value = item.MeasuredPlotArea;
                    /*worksheet.Cells[row, 9].Value = item.MeasuredPageNumber;*/
                    worksheet.Cells[row, 9].Value = item.WithdrawArea;




                    worksheet.Cells[6, 3].Value = item.ProjectName;

                    row++;
                    sttCounter++;
                }

                // Save to the temporary file
                package.SaveAs(tempFile);
            }


            fileBytes = File.ReadAllBytes(tempFile.FullName);


            File.Delete(tempFile.FullName);


            var exportFileName = $"{Path.GetFileNameWithoutExtension(templateFileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(templateFileName)}";

            return new ExportFileDTO
            {
                FileName = exportFileName,
                FileByte = fileBytes,
                FileType = FileTypeExtensions.ToFileMimeTypeString(filetype)
            };
        }

        /// <summary>
        /// Phuong An Bao Cao File Doc
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task<ExportFileDTO> ExportPlanReportsWordAsync(string planId, FileTypeEnum filetype = FileTypeEnum.docx)
        {
            //Get Data BTHT
            var dataBTHT = await GetDataForBTHTPlanAsync(planId)
                ?? throw new Exception("Value is null");

            //Get File Template
            var fileName = _getFileTemplateDirectory.Get("PhuongAn_BaoCao");

            var a = Path.Combine(_getFileTemplateDirectory.GetStoragePath(), "Temp");
            //Create new File Based on Template
            var fileDest = Path.Combine(
                _getFileTemplateDirectory.GetStoragePath(), "Temp",
                $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(fileName)}");

            if (!CopyTemplate(fileName, fileDest)) throw new InvalidActionException("Cannot Create File");

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
                //wordDoc.Close();
                wordDoc.Dispose();
            }


            byte[] fileBytes;

            if (filetype == FileTypeEnum.pdf)
            {
                fileBytes = await ConvertDocxToPdf(fileDest);
            }

            else if (filetype == FileTypeEnum.docx)
            {
                fileBytes = File.ReadAllBytes(fileDest);
            }

            else
            {
                throw new InvalidActionException($"Unsupported file type: {filetype}");
            }

            File.Delete(fileDest);

            return new ExportFileDTO
            {
                FileName = Path.GetFileNameWithoutExtension(fileDest),
                FileByte = fileBytes,
                FileType = FileTypeExtensions.ToFileMimeTypeString(filetype) // Change this to the appropriate content type for Word documents
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
                        var text = new Text("*Created By Yolo Team");
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
        /// Use this method to check all assets value of plan , save changed if apply change = true
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="applyChanged"></param>
        /// <returns></returns>
        public async Task<PlanReadDTO> ReCheckPricesOfPlanAsync(string planId, bool applyChanged = false)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);
            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);
            //1.Caculate all owner of a plan with status isDelete = false, => Sum total_owner_support_compensation
            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId);
            plan.TotalOwnerSupportCompensation = owners.Count();
            //2.For each owner, re-caculating related prices and reassign it to plan

            plan.TotalPriceCompensation = 0;

            plan.TotalPriceLandSupportCompensation = 0;

            plan.TotalPriceHouseSupportCompensation = 0;

            plan.TotalPriceArchitectureSupportCompensation = 0;

            plan.TotalPricePlantSupportCompensation = 0;

            plan.TotalDeduction = 0;

            plan.TotalLandRecoveryArea = 0;

            plan.TotalGpmbServiceCost = 0;

            foreach (var owner in owners)
            {
                plan.TotalPriceCompensation = plan.TotalPriceCompensation + _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, null, true).Result
                                    + _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId, true).Result;

                plan.TotalPriceLandSupportCompensation += _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId, true).Result;

                plan.TotalPriceHouseSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.House, true).Result;

                plan.TotalPriceArchitectureSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Architecture, true).Result;

                plan.TotalPricePlantSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Plants, true).Result;

                plan.TotalDeduction += _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(owner.OwnerId).Result;

                plan.TotalLandRecoveryArea += _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(owner.OwnerId).Result;

            }


            plan.TotalGpmbServiceCost += plan.TotalGpmbServiceCost += plan.TotalPriceLandSupportCompensation
                + plan.TotalPriceHouseSupportCompensation
                + plan.TotalPriceArchitectureSupportCompensation
                + plan.TotalPricePlantSupportCompensation + plan.TotalDeduction;


            if (!applyChanged)
            {
                await _unitOfWork.CommitAsync();
            }

            return _mapper.Map<PlanReadDTO>(plan);
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
                ?? throw new EntityWithAttributeNotFoundException<Core.Entities.Owner>(nameof(Plan.PlanId), planId);

            throw new NotImplementedException();
        }


        public async Task<IEnumerable<PlanReadDTO>> GetPlansOfProjectASync(string projectId)
        {
            return _mapper.Map<IEnumerable<PlanReadDTO>>(await _unitOfWork.PlanRepository.GetPlansOfProjectAsync(projectId));
        }

        public async Task<PlanReadDTO> ApprovePlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "Owners");

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.AWAITING.ToString())
            {
                throw new InvalidActionException($"Plan With Status [{plan.PlanStatus}] Is Not Valid To Approve. Must Be [{PlanStatusEnum.AWAITING}]");
            }

            //check all owner must be status accept compensation before accept plans
            //- if performance issue: maybe no need cause SendPlanApproveRequestAsync did check
            foreach (var owner in plan.Owners)
            {
                if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
                    throw new InvalidActionException($"Owner {owner.OwnerId} with Name: {owner.OwnerName} who have Status: {owner.OwnerStatus} that is invalid to approve plan");
            }


            plan.PlanStatus = PlanStatusEnum.APPROVED.ToString();

            await _unitOfWork.CommitAsync();

            //await _notificationService.SendNotificationToUserAsync(plan.PlanApprovedBy, "Approve Plan Success", $"Plan with code: {plan.PlanCode!} successfully approved ");

            return _mapper.Map<PlanReadDTO>(plan);
        }


        public async Task<PlanReadDTO> SendPlanApproveRequestAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.DRAFT.ToString())
            {
                throw new InvalidActionException($"Plan With Status [{plan.PlanStatus}] Is Not Valid To Send Approve Request. Must Be [{PlanStatusEnum.DRAFT}]");
            }

            //check all owner must be status accept compensation before send plan approve request
            foreach (var owner in plan.Owners)
            {
                if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
                    throw new InvalidActionException($"Owner {owner.OwnerId} with Name: {owner.OwnerName} who have Status: {owner.OwnerStatus} that is invalid to approve plan");
            }


            plan.PlanStatus = PlanStatusEnum.AWAITING.ToString();

            await _unitOfWork.CommitAsync();

            // await _notificationService.SendNotificationToUserAsync(plan.PlanApprovedBy, "Approve Plan Success", $"Plan with code: {plan.PlanCode!} successfully approved ");

            return _mapper.Map<PlanReadDTO>(plan);
        }

        public async Task<PlanReadDTO> RejectPlanAsync(string planId, string reason)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.AWAITING.ToString())
            {
                throw new InvalidActionException($"Plan With Status [{plan.PlanStatus}] Is Not Valid To Reject. Must Be [{PlanStatusEnum.AWAITING}]");
            }

            plan.PlanStatus = PlanStatusEnum.REJECTED.ToString();

            plan.RejectReason = reason;

            await _unitOfWork.CommitAsync();

            //await _notificationService.SendNotificationToUserAsync(plan.PlanApprovedBy, "Approve Plan Success", $"Plan with code: {plan.PlanCode!} successfully approved ");

            return _mapper.Map<PlanReadDTO>(plan);
        }

        public async Task<PlanReadDTO> CreatePlanCopyAsync(string planId)
        {
            var originalPlan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "AttachFiles, Owners", trackChanges: false);

            if (originalPlan == null) throw new EntityWithIDNotFoundException<Plan>(planId);


            var newPlan = new Plan();

            newPlan = originalPlan;

            //Assign Version Copy
            if (originalPlan.PlanCode.Contains(" - Copy"))
            {
                var parts = originalPlan.PlanCode.Split(" - Copy");

                // Extract the base plan code without the version number
                var basePlanCode = parts[0].Trim();

                // Extract the version number (if any)
                var versionNumber = 1;
                if (parts.Length > 1 && parts[1].Contains("("))
                {
                    var versionString = parts[1]
                        .Split('(')[1]
                        .Split(')')[0]
                        .Trim();

                    if (int.TryParse(versionString, out var parsedVersion))
                    {
                        versionNumber = parsedVersion + 1;
                    }
                }

                // Create the new plan code with the incremented version number
                newPlan.PlanCode = $"{basePlanCode} - Copy ({versionNumber})";
            }
            else
            {
                // If the original plan code doesn't contain " - Copy," simply append it
                newPlan.PlanCode = $"{originalPlan.PlanCode} - Copy";
            }

            if (newPlan.PlanCode.Count() > 50)
            {
                throw new InvalidActionException($"Cannot Create Plan Copy. Plan Code Exceed 50 Characters");
            }

            newPlan.PlanId = Guid.NewGuid().ToString();

            newPlan.PlanStatus = PlanStatusEnum.DRAFT.ToString();

            newPlan.PlanCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            if (!newPlan.RejectReason.IsNullOrEmpty())
            {
                newPlan.RejectReason = "";
            }


            foreach (var file in newPlan.AttachFiles)
            {
                file.AttachFileId = Guid.NewGuid().ToString();
                file.CreatedBy = _userContextService.Username!
                    ?? throw new CanNotAssignUserException();
            }



            foreach (var oldOwner in originalPlan.Owners)
            {
                await _ownerService.DeleteOldOwnerWhenCreatePlanCopy(oldOwner.OwnerId);
            }


            foreach (var newOwner in newPlan.Owners)
            {
                newOwner.OwnerId = Guid.NewGuid().ToString();
                newOwner.PlanId = newPlan.PlanId;
                newOwner.OwnerCode = newOwner.OwnerCode + "- Copy from " + newPlan.PlanCode;
                newOwner.OwnerStatus = OwnerStatusEnum.Unknown.ToString();
                newOwner.OwnerCreatedTime = DateTime.Now.SetKindUtc();
                newOwner.IsDeleted = false;
                newOwner.OwnerCreatedBy = _userContextService.Username!
                    ?? throw new CanNotAssignUserException();
            }

            await _unitOfWork.PlanRepository.AddAsync(newPlan);


            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(newPlan);

        }

        public async Task<PaginatedResponse<PlanReadDTO>> QueryPlansOfCreatorAsync(PlanQuery query, PlanStatusEnum? planStatus)
        {
            var currentCreatorName = _userContextService.Username!
                ?? throw new InvalidActionException("Cannot Define Creator From Context");

            var plan = await _unitOfWork.PlanRepository.QueryPlanOfCreatorAsync(query, currentCreatorName, planStatus);

            return PaginatedResponse<PlanReadDTO>.FromEnumerableWithMapping(plan, query, _mapper);
        }



    }
}
