﻿using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
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

        public Task<ExportFileDTO> ExportBTHTPlansPdfAsync(string planId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use this method to check and reassign prices value of plan when price settings or owners of plan were changed
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public async Task ReCheckPricesOfPlanAsync(string planId)
        {
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);
            if(plan == null) throw new EntityWithIDNotFoundException<Plan>(planId);
            //1.Caculate all owner of a plan with status isDelete = false, => Sum total_owner_support_compensation
            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfPlanAsync(planId);
            plan.TotalOwnerSupportCompensation = owners.Count();
            //2.For each owner, re-caculating related prices and reassign it to plan
        }
    }
}
