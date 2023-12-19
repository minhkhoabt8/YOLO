using Amazon.S3.Model;
using Aspose.Pdf.Operators;
using AutoMapper;
using BitMiracle.LibTiff.Classic;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Api.Gax.ResourceNames;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.AttachFile;
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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using Xceed.Words.NET;
using SharedLib.Infrastructure.Repositories.Interfaces;

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
        private readonly IDigitalSignatureService _digitalSignatureService;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IAttachFileService attachFileService, IAuthService authService, IOwnerService ownerService, IGetFileTemplateDirectory getFileTemplateDirectory, IDigitalSignatureService digitalSignatureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _attachFileService = attachFileService;
            _authService = authService;
            _ownerService = ownerService;
            _getFileTemplateDirectory = getFileTemplateDirectory;
            _digitalSignatureService = digitalSignatureService;
        }

        //api check not allow duplicate plan code
        public async Task<bool> CheckDuplicatePlanCodeAsync(string planCode)
        {
            var plan = await _unitOfWork.PlanRepository.GetPlanByPlanCodeAsync(planCode);
            if (plan != null)
            {
                throw new UniqueConstraintException<Project>(nameof(plan.PlanCode), planCode);
            }
            return true;
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
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include:"Owners");

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus == PlanStatusEnum.REJECTED.ToString()
                || plan.PlanStatus == PlanStatusEnum.AWAITING.ToString()
                || plan.PlanStatus == PlanStatusEnum.APPROVED.ToString())
            {
                throw new InvalidActionException($"Không thể xóa phương án với trạng thái [{plan.PlanStatus}].");
            }

            if (plan.Owners.Any())
            {
                throw new InvalidActionException($"Không thể xóa phương án đã tồn tại Chủ sở hữu.");
            }

            plan.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<PlanReadDTO> GetPlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "AttachFiles");
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

            if (dto.PlanCode!.ToLower() != plan.PlanCode.ToLower()) 
            {
                var duplicatePlan = await _unitOfWork.PlanRepository.GetPlanByPlanCodeAsync(dto.PlanCode!);

                if (duplicatePlan != null)
                {
                    throw new UniqueConstraintException("Có một phương án khác đã tồn tại trong hệ thống");
                }
            }

            if (plan.PlanStatus == PlanStatusEnum.REJECTED.ToString()
                || plan.PlanStatus == PlanStatusEnum.AWAITING.ToString()
                || plan.PlanStatus == PlanStatusEnum.APPROVED.ToString())
            {
                throw new InvalidActionException($"Không thể cập nhật phương án đang có chủ sở hữu có trạng thái [{plan.PlanStatus}].");
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
                TotalGpmbServiceCost = plan.TotalGpmbServiceCost,// needd updated
                TotalOwnerSupportPrice = plan.TotalOwnerSupportPrice
            };
        }


        //API mới
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
            int stt = 0;
            foreach (var item in owners)
            {
                var measuredLandInfos = await _unitOfWork.MeasuredLandInfoRepository.GetAllMeasuredLandInfosOfOwnerAsync(item.OwnerId)
                ?? throw new EntityWithAttributeNotFoundException<MeasuredLandInfo>(nameof(Core.Entities.Owner.OwnerId), item.OwnerId);

                string measuredPageNumbers = "";
                string measuredPlotNumbers = "";
                decimal mesuredPlotArea =0;
                decimal withDrawalArea =0;
                string codeLandTypes = "";
                foreach (var i in measuredLandInfos)
                {
                    measuredPageNumbers += (measuredPageNumbers.Length > 0 ? ", " : "") + i.MeasuredPageNumber;
                    measuredPlotNumbers += (measuredPlotNumbers.Length > 0 ? ", " : "") + i.MeasuredPlotNumber;
                    

                    var laneType = await _unitOfWork.LandTypeRepository.GetLandTypesOfMeasureLandInfoAsync(i.LandTypeId);
                    codeLandTypes += (codeLandTypes.Length > 0 ? ", " : "") + laneType.Code;
                    mesuredPlotArea = i.MeasuredPlotArea ?? 0 ;
                    withDrawalArea = i.WithdrawArea ?? 0;
                }
                DetailBTHChiPhiReadDTO detail = new DetailBTHChiPhiReadDTO();
                detail.Stt = ++stt;
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
                detail.SumLandResettlementPrice = await _unitOfWork.LandResettlementRepository.CaculateTotalLandPricesOfOwnerAsync(item.OwnerId);
                //
                detail.MeasuredPageNumber = measuredPageNumbers;
                detail.MeasuredPlotNumber = measuredPlotNumbers;
                
                detail.CodeLandType = codeLandTypes;
                //
                detail.MeasuredPlotArea = mesuredPlotArea.ToString();
                detail.WithdrawArea = withDrawalArea;

                detail.SumLandCompensation = await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(item.OwnerId, true);
                detail.SumHouseCompensationPrice = await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(item.OwnerId, AssetOnLandTypeEnum.House);
                detail.SumTreeCompensationPrice = await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(item.OwnerId, AssetOnLandTypeEnum.Plants);
                detail.SumArchitectureCompensationPrice = await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(item.OwnerId, AssetOnLandTypeEnum.Architecture);
                detail.SumSupportPrice = await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(item.OwnerId);
                detail.SumDeductionPrice = await _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(item.OwnerId);
                detail.SumBTHT = (decimal)(detail.SumLandCompensation + detail.SumHouseCompensationPrice + detail.SumTreeCompensationPrice + detail.SumArchitectureCompensationPrice
                    + detail.SumSupportPrice - detail.SumDeductionPrice);
                details.Add(detail);

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

            var templateFileName = _getFileTemplateDirectory.GetExport("BangTongHopChiPhiBT");
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

            var templateFileName = _getFileTemplateDirectory.GetExport("BangTongHopThuHoi");
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
            var fileName = _getFileTemplateDirectory.GetExport("PhuongAn_BaoCao");

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
                                text.Text = text.Text.Replace("boithuongkhac", string.Format("{0:#,##0đ}", dataBTHT.TotalOwnerSupportPrice));
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

            plan.TotalOwnerSupportPrice = 0;

            plan.TotalGpmbServiceCost = 0;

            foreach (var owner in owners)
            {
                var serviceCostOfOwner = (decimal)((double)(await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId)
                 + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.House)
                 + await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId)
                 + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Architecture)
                 + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Plants))
                 * 0.02);


                plan.TotalPriceLandSupportCompensation += _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(owner.OwnerId, true).Result;

                plan.TotalPriceHouseSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.House, true).Result;

                plan.TotalPriceArchitectureSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Architecture, true).Result;

                plan.TotalPricePlantSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(owner.OwnerId, AssetOnLandTypeEnum.Plants, true).Result;

                plan.TotalDeduction += _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(owner.OwnerId).Result;

                plan.TotalLandRecoveryArea += _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(owner.OwnerId).Result;

                plan.TotalOwnerSupportPrice += _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(owner.OwnerId).Result;

                plan.TotalGpmbServiceCost += serviceCostOfOwner;

                plan.TotalPriceCompensation = plan.TotalPriceLandSupportCompensation
                + plan.TotalPriceHouseSupportCompensation
                + plan.TotalPriceArchitectureSupportCompensation
                + plan.TotalPricePlantSupportCompensation
                + plan.TotalOwnerSupportPrice
                + plan.TotalGpmbServiceCost;

            }

            //plan.TotalGpmbServiceCost += plan.TotalGpmbServiceCost += plan.TotalPriceLandSupportCompensation
            //    + plan.TotalPriceHouseSupportCompensation
            //    + plan.TotalPriceArchitectureSupportCompensation
            //    + plan.TotalPricePlantSupportCompensation + plan.TotalDeduction;

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


        public async Task<PaginatedResponse<PlanReadDTO>> QueryPlansOfProjectAsync(string? projectId, PlanQuery query)
        {
            var plan = await _unitOfWork.PlanRepository.QueryPlansOfProjectAsync(projectId, query);

            return PaginatedResponse<PlanReadDTO>.FromEnumerableWithMapping(plan, query, _mapper);
        }

        public async Task<IEnumerable<PlanReadDTO>> GetPlansOfProjectAsync(string projectId)
        {
            return _mapper.Map<IEnumerable<PlanReadDTO>>(await _unitOfWork.PlanRepository.GetPlansOfProjectAsync(projectId));
        }


        public async Task<PlanReadDTO> ApprovePlanAsync(string planId, string signaturePassword, IFormFile signingFile)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "Owners");

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.AWAITING.ToString())
            {
                throw new InvalidActionException($"Phương án với trạng thái [{plan.PlanStatus}] không hợp lệ. Bắt buộc phải là [{PlanStatusEnum.AWAITING}]");
            }

            //check all owner must be status accept compensation before accept plans
            //- if performance issue: maybe no need cause SendPlanApproveRequestAsync did check
            //foreach (var owner in plan.Owners)
            //{
            //    if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
            //        throw new InvalidActionException($"Chủ sở hữu có mã: [{owner.OwnerCode}] với trạng thái: {owner.OwnerStatus} không hợp lệ.");
            //}
            var signerId = _userContextService.AccountID!
                ?? throw new CanNotAssignUserException();

            if(signerId != plan.PlanApprovedBy) 
            {
                throw new ForbiddenException("Current user is not authorized to perform this action.");
            }

            var file = await _digitalSignatureService.SignDocumentWithPictureAsync(plan.PlanApprovedBy ?? signerId, signingFile, signaturePassword, true);

            var attachFile = new AttachFileWriteDTO
            {
                PlanId = plan.PlanId,
                IsAssetCompensation = false,
                AttachFile = file.FileByte,
                FileType = FileTypeEnum.pdf,
                Name = file.FileName
            };

            var attachFileRead = await _attachFileService.UploadSignedPdfAttachFileAsync(attachFile);

            plan.PlanStatus = PlanStatusEnum.APPROVED.ToString();

            await _unitOfWork.CommitAsync();

            var result =  _mapper.Map<PlanReadDTO>(plan);

            result.AttachFiles.Add(attachFileRead);

            return result;
        }

        public async Task<PlanReadDTO> ApprovePlanWithSignedDocumentAsync(string planId, IFormFile signedFile)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "Owners");

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.AWAITING.ToString())
            {
                throw new InvalidActionException($"Phương án với trạng thái [{plan.PlanStatus}] không hợp lệ. Bắt buộc phải là [{PlanStatusEnum.AWAITING}]");
            }

            var signerId = _userContextService.AccountID!
                ?? throw new CanNotAssignUserException();

            if (signerId != plan.PlanApprovedBy)
            {
                throw new ForbiddenException("Current user is not authorized to perform this action.");
            }

            //check all owner must be status accept compensation before accept plans
            //- if performance issue: maybe no need cause SendPlanApproveRequestAsync did check
            foreach (var owner in plan.Owners)
            {
                if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
                    throw new InvalidActionException($"Chủ sở hữu có mã: [{owner.OwnerCode}] với trạng thái: {owner.OwnerStatus} không hợp lệ.");

            }


            var isSigned = await _digitalSignatureService.VerifySignedDocument(signedFile);

            if (!isSigned)
            {
                throw new InvalidActionException("Cannot Verify File Signature");
            }

            var attachFile = new AttachFileWriteDTO
            {
                PlanId = plan.PlanId,
                IsAssetCompensation = false,
                AttachFile = ReadIFormFileAsBytes(signedFile),
                FileType = FileTypeEnum.pdf,
                Name = signedFile.FileName
            };

            var attachFileRead = await _attachFileService.UploadSignedPdfAttachFileAsync(attachFile);

            plan.PlanStatus = PlanStatusEnum.APPROVED.ToString();

            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<PlanReadDTO>(plan);

            result.AttachFiles.Add(attachFileRead);

            return result;
        }


        public async Task<PlanReadDTO> ApprovePlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "Owners");

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.AWAITING.ToString())
            {
                throw new InvalidActionException($"Phương án với trạng thái [{plan.PlanStatus}] không hợp lệ. Bắt buộc phải là [{PlanStatusEnum.AWAITING}]");
            }

            //check all owner must be status accept compensation before accept plans
            //- if performance issue: maybe no need cause SendPlanApproveRequestAsync did check
            //foreach (var owner in plan.Owners)
            //{
            //    if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
            //        throw new InvalidActionException($"Chủ sở hữu có mã: [{owner.OwnerCode}] với trạng thái: {owner.OwnerStatus} không hợp lệ.");

            //}

            plan.PlanStatus = PlanStatusEnum.APPROVED.ToString();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(plan);
        }


        public async Task<PlanReadDTO> SendPlanApproveRequestAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (!plan.PlanStatus.Contains(PlanStatusEnum.DRAFT.ToString()))
            {
                throw new InvalidActionException($"Phương án với trạng thái [{plan.PlanStatus}] không hợp lệ. Bắt buộc phải là [{PlanStatusEnum.DRAFT}]");
            }

            //check all owner must be status accept compensation and PlanEnded Time is <=Datatimr.Now  before send plan approve request
            foreach (var owner in plan.Owners)
            {
                //if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
                //    throw new InvalidActionException($"Owner {owner.OwnerId} with Name: {owner.OwnerName} who have Status: {owner.OwnerStatus} that is invalid to approve plan");

                if (owner.OwnerStatus!.Equals(OwnerStatusEnum.Unknown.ToString()) || owner.OwnerStatus!.Equals(OwnerStatusEnum.RejectCompensation.ToString()))
                {
                    if (plan.PlanEndedTime < DateTime.Now.SetKindUtc())
                    {
                        throw new InvalidActionException($"Không thể gửi yêu cầu vì phương án chưa hết hạn và vẫn còn chủ sở hữu: [{owner.OwnerCode}] có trạng thái: [{owner.OwnerStatus}] không hợp lệ.");
                    }
                }
                    
            }


            plan.PlanStatus = PlanStatusEnum.AWAITING.ToString();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(plan);
        }

        public async Task<PlanReadDTO> RejectPlanAsync(string planId, string reason)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId, include:"Owners");

            if (plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if (plan.PlanStatus != PlanStatusEnum.AWAITING.ToString())
            {
                throw new InvalidActionException($"Phương án với trạng thái [{plan.PlanStatus}] không hợp lệ. Bắt buộc phải là [{PlanStatusEnum.AWAITING}]");
            }

            var signerId = _userContextService.AccountID!
                ?? throw new CanNotAssignUserException();

            if (signerId != plan.PlanApprovedBy)
            {
                throw new ForbiddenException("Current user is not authorized to perform this action.");
            }

            if(!plan.Owners.IsNullOrEmpty())
            {
                foreach(var owner in plan.Owners)
                {
                    owner.OwnerStatus = OwnerStatusEnum.Unknown.ToString();
                }
            }

            plan.PlanStatus = PlanStatusEnum.REJECTED.ToString();

            plan.RejectReason = reason;

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(plan);
        }

        public async Task<PlanReadDTO> CreatePlanCopyAsync(string planId)
        {
            var originalPlan = await _unitOfWork.PlanRepository.FindAsync(planId, include: "AttachFiles, Owners");

            if (originalPlan == null) throw new EntityWithIDNotFoundException<Plan>(planId);

            if(!originalPlan.PlanStatus.Contains(PlanStatusEnum.REJECTED.ToString()))
            {
                throw new InvalidActionException($"Phương án [{originalPlan.PlanCode}] với trạng thái: [{originalPlan.PlanStatus}] không thể tạo bản sao.");
            }

            var newPlan = CreatePlanCopy(originalPlan);

            newPlan.PlanCode = GeneratePlanCode(originalPlan.PlanCode);

            if (newPlan.PlanCode.Count() > 50)
            {
                throw new InvalidActionException($"Không thể tạo thêm bản sao. Mã phương án vượt quá 50 kí tự.");
            }

            //newPlan.AttachFiles = originalPlan.AttachFiles.Select(CreateNewAttachFile).ToList();

            //newPlan.AttachFiles = originalPlan.AttachFiles.Select(file =>
            //{
            //    var newFile = new AttachFile
            //    {
            //        AttachFileId = Guid.NewGuid().ToString(),
            //        CreatedBy = _userContextService.Username!
            //                ?? throw new CanNotAssignUserException()
            //    };
            //    return newFile;
            //}).ToList();

            await _unitOfWork.PlanRepository.AddAsync(newPlan);

            foreach (var oldOwner in originalPlan.Owners)
            {
                //Only Copy Owner That Not Delete
                if(!oldOwner.IsDeleted)
                {
                    oldOwner.PlanId = newPlan.PlanId;
                    oldOwner.OwnerStatus = OwnerStatusEnum.Unknown.ToString();


                    var serviceCostOfOwner = (decimal)((double)(await _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(oldOwner.OwnerId)
                     + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(oldOwner.OwnerId, AssetOnLandTypeEnum.House)
                     + await _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(oldOwner.OwnerId)
                     + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(oldOwner.OwnerId, AssetOnLandTypeEnum.Architecture)
                     + await _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(oldOwner.OwnerId, AssetOnLandTypeEnum.Plants))
                     * 0.02);

                    newPlan.TotalOwnerSupportCompensation += 1;

                    newPlan.TotalPriceLandSupportCompensation += _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandCompensationPriceOfOwnerAsync(oldOwner.OwnerId, true).Result;

                    newPlan.TotalPriceHouseSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(oldOwner.OwnerId, AssetOnLandTypeEnum.House, true).Result;

                    newPlan.TotalPriceArchitectureSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(oldOwner.OwnerId, AssetOnLandTypeEnum.Architecture, true).Result;

                    newPlan.TotalPricePlantSupportCompensation += _unitOfWork.AssetCompensationRepository.CaculateTotalAssetCompensationOfOwnerAsync(oldOwner.OwnerId, AssetOnLandTypeEnum.Plants, true).Result;

                    newPlan.TotalDeduction += _unitOfWork.DeductionRepository.CaculateTotalDeductionOfOwnerAsync(oldOwner.OwnerId).Result;

                    newPlan.TotalLandRecoveryArea += _unitOfWork.MeasuredLandInfoRepository.CaculateTotalLandRecoveryAreaOfOwnerAsync(oldOwner.OwnerId).Result;

                    newPlan.TotalOwnerSupportPrice += _unitOfWork.SupportRepository.CaculateTotalSupportOfOwnerAsync(oldOwner.OwnerId).Result;

                    newPlan.TotalGpmbServiceCost += serviceCostOfOwner;

                    newPlan.TotalPriceCompensation = newPlan.TotalPriceLandSupportCompensation
                    + newPlan.TotalPriceHouseSupportCompensation
                    + newPlan.TotalPriceArchitectureSupportCompensation
                    + newPlan.TotalPricePlantSupportCompensation
                    + newPlan.TotalOwnerSupportPrice
                    + newPlan.TotalGpmbServiceCost;
                }
               
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(newPlan);

        }

        private Plan CreatePlanCopy(Plan originalPlan)
        {
            return new Plan
            {
                PlanId = Guid.NewGuid().ToString(),
                ProjectId = originalPlan.ProjectId,
                PlanReportInfo = originalPlan.PlanReportInfo,
                PlanCode = originalPlan.PlanCode,
                PlanDescription = originalPlan.PlanDescription,
                PlanCreateBase = originalPlan.PlanCreateBase,
                PlanApprovedBy = originalPlan.PlanApprovedBy,
                PlanReportSignal = originalPlan.PlanReportSignal,
                PlanReportDate = originalPlan.PlanReportDate,
                PlanCreatedTime = originalPlan.PlanCreatedTime,
                PlanEndedTime = originalPlan.PlanEndedTime,
                PlanCreatedBy = originalPlan.PlanCreatedBy,
                PlanStatus = PlanStatusEnum.DRAFT.ToString(),
                RejectReason = originalPlan.RejectReason ?? "",
                TotalOwnerSupportCompensation = 0,
                TotalPriceCompensation = 0,
                TotalPriceLandSupportCompensation = 0,
                TotalPriceHouseSupportCompensation = 0,
                TotalPriceArchitectureSupportCompensation = 0,
                TotalPricePlantSupportCompensation = 0,
                TotalDeduction = 0,
                TotalLandRecoveryArea = 0,
                TotalOwnerSupportPrice = 0,
                TotalGpmbServiceCost = 0,
                IsDeleted = false
            };
        }
        private string GeneratePlanCode(string originalPlanCode)
        {
            if (originalPlanCode.Contains(" - Copy"))
            {
                var parts = originalPlanCode.Split(" - Copy");

                var basePlanCode = parts[0].Trim();
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

                return $"{basePlanCode} - Copy ({versionNumber})";
            }
            else
            {
                return $"{originalPlanCode} - Copy";
            }
        }

        private AttachFile CreateNewAttachFile(AttachFile originalFile)
        {
            return new AttachFile
            {
                AttachFileId = Guid.NewGuid().ToString(),
                CreatedBy = _userContextService.Username! ?? throw new CanNotAssignUserException()
            };
        }

        public async Task<PaginatedResponse<PlanReadDTO>> QueryPlansOfCreatorAsync(PlanQuery query,string? creatorName = null, PlanStatusEnum? planStatus = null)
        {
            if (creatorName != null) 
            {
                var plan = await _unitOfWork.PlanRepository.QueryPlanOfCreatorAsync(query, creatorName, planStatus);
                return PaginatedResponse<PlanReadDTO>.FromEnumerableWithMapping(plan, query, _mapper);
            }
            else
            {
                var currentCreatorName = _userContextService.Username!
                ?? throw new InvalidActionException("Cannot Define Creator From Context");

                var plan = await _unitOfWork.PlanRepository.QueryPlanOfCreatorAsync(query, currentCreatorName, planStatus);
                return PaginatedResponse<PlanReadDTO>.FromEnumerableWithMapping(plan, query, _mapper);
            }
        }

        public async Task<PaginatedResponse<PlanReadDTO>> QueryPlanOfApprovalAsync(PlanQuery query, PlanStatusEnum? planStatus = null)
        {
            var currentCreatorId = _userContextService.AccountID!
                ?? throw new InvalidActionException("Cannot Define Approval From Context");

            var plan = await _unitOfWork.PlanRepository.QueryPlanOfApprovalAsync(query, currentCreatorId, planStatus);

            return PaginatedResponse<PlanReadDTO>.FromEnumerableWithMapping(plan, query, _mapper);
        }

        public byte[] ReadIFormFileAsBytes(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Perform a deep copy of the object via serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>A deep copy of the object.</returns>
        public T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            if (ReferenceEquals(source, null)) return default;

            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
