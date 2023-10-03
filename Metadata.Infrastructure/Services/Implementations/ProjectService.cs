using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IDocumentService _documentService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IDocumentService documentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _documentService = documentService;
        }

        public async Task<ProjectReadDTO> CreateProjectAsync(ProjectWriteDTO projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            project.ProjectCreatedBy = _userContextService.Username! 
                ?? throw new CanNotAssignUserException();

            if(projectDto.Documents.IsNullOrEmpty())
            {
                await _documentService.AssignDocumentsToProjectAsync(project.ProjectId, projectDto.Documents!);
            }

            await _unitOfWork.ProjectRepository.AddAsync(project);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProjectReadDTO>(project);
        }

        public Task CreateProjectsFromFileAsync(IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProjectAsync(string projectId)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId);

            if(project == null) throw new EntityWithIDNotFoundException<Project>(projectId);

            project.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<ProjectReadDTO>> GetAllProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectReadDTO> GetProjectAsync(string projectId)
        {
            var project = await _unitOfWork.ProjectRepository
                .FindAsync(projectId, include: "LandPositionInfos, Owners, Plans, PriceAppliedCode, UnitPriceLands, ProjectDocuments");
            
            var projectReadDto =  _mapper.Map<ProjectReadDTO>(project);

            var projectDocuments = await _unitOfWork.DocumentRepository.GetDocumentsOfProjectAsync(projectId);

            projectReadDto.Documents = _mapper.Map<IEnumerable<DocumentReadDTO>>(projectDocuments);

            return projectReadDto;
        }

        public async Task<PaginatedResponse<ProjectReadDTO>> ProjectQueryAsync(ProjectQuery query)
        {
            var projects = await _unitOfWork.ProjectRepository.QueryAsync(query);

            return PaginatedResponse<ProjectReadDTO>.FromEnumerableWithMapping(projects, query, _mapper);
        }

        public async Task<ProjectReadDTO> UpdateProjectAsync(string projectId, ProjectWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId);

            if(project == null) throw new EntityWithIDNotFoundException<Project>(projectId);

            _mapper.Map(dto, project);

            return _mapper.Map<ProjectReadDTO>(project);
        }
    }
}
