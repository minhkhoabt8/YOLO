using Metadata.Core.Enums;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IPlanService
    {
        Task<PlanReadDTO> GetPlanAsync(string planId);
        Task<PaginatedResponse<PlanReadDTO>> QueryPlanAsync(PlanQuery query);
        Task<PlanReadDTO> CreatePlanAsync(PlanWriteDTO dto);
        Task<PlanReadDTO> UpdatePlanAsync(string planId, PlanWriteDTO dto);
        Task DeletePlan(string planId);
        Task ImportPlan(IFormFile attachFile);
        Task<ExportFileDTO> ExportPlansFileAsync(string projectId);
        Task<ExportFileDTO> ExportPlanReportsWordAsync(string planId, FileTypeEnum filetype = FileTypeEnum.docx);
        //Bảng Tổng Hợp Thu Hồi 
        Task<ExportFileDTO> ExportSummaryOfRecoveryExcelAsync(string planId);
        Task<PlanReadDTO> ReCheckPricesOfPlanAsync(string planId, bool applyChanged = false);
        Task<IEnumerable<PlanReadDTO>> GetPlansOfProjectAsync(string projectId);
        Task<PaginatedResponse<PlanReadDTO>> QueryPlansOfProjectAsync(string? projectId, PlanQuery query);
        //Bảng tổng hợp chi phí
        Task<List<DetailBTHChiPhiReadDTO>> getDataForBTHChiPhiAsync(string planId);

        Task<PlanReadDTO> ApprovePlanAsync(string planId);

        Task<PlanReadDTO> ApprovePlanAsync(string planId, string signaturePassword, IFormFile signedFile);

        Task<PlanReadDTO> ApprovePlanWithSignedDocumentAsync(string planId, IFormFile signedFile);

        Task<PlanReadDTO> CreatePlanCopyAsync(string planId);

        Task<PlanReadDTO> RejectPlanAsync(string planId, string reason);

        Task<PlanReadDTO> SendPlanApproveRequestAsync(string planId);

        Task<PaginatedResponse<PlanReadDTO>> QueryPlansOfCreatorAsync(PlanQuery query, string? creatorName = null, PlanStatusEnum? planStatus = null);
        Task<PaginatedResponse<PlanReadDTO>> QueryPlanOfApprovalAsync(PlanQuery query, PlanStatusEnum? planStatus = null);
        Task<ExportFileDTO> ExportBTHChiPhiToExcelAsync(string planId, FileTypeEnum filetype = FileTypeEnum.docx);
        Task<ExportFileDTO> ExportBTHThuHoiToExcelAsync(string planId, FileTypeEnum filetype = FileTypeEnum.xlsx);

        Task<bool> CheckDuplicatePlanCodeAsync(string planCode);

    }
}
