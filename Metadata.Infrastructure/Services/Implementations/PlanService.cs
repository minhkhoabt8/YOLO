﻿using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
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

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<PlanReadDTO> CreatePlanAsync(PlanWriteDTO dto)
        {
            var plan = _mapper.Map<Plan>(dto);

            plan.PlanCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.PlanRepository.AddAsync(plan);

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
            var plan = await _unitOfWork.PlanRepository.FindAsync(planId);

            if (plan == null) throw new EntityWithIDNotFoundException<Owner>(planId);

            _mapper.Map(dto, plan);

            plan.PlanCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlanReadDTO>(plan);
        }
    }
}
