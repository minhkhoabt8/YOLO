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
        Task<ExportFileDTO> ExportBTHTPlansWordAsync(string planId);
        Task ReCheckPricesOfPlanAsync(string planId);
    }
}
