using Amazon.Runtime.Internal.Auth;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Document;
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
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IDocumentService _documentService;
        private readonly IAuthService _authService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IDocumentService documentService, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _documentService = documentService;
            _authService = authService;
        }

        public async Task<ProjectReadDTO> CreateProjectAsync(ProjectWriteDTO projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            project.ProjectCreatedBy = _userContextService.Username! ??
                throw new CanNotAssignUserException();

            if(!project.SignerId.IsNullOrEmpty())
            {
                var signer = await _authService.GetAccountByIdAsync(project.SignerId!);

                if (signer == null || signer.Role.Id != ((int)AuthRoleEnum.Approval).ToString())
                {
                    throw new CannotAssignSignerException();
                }

                project.SignerId = signer.Id;
            }
            
            if (!projectDto.LandPositionInfos.IsNullOrEmpty())
            {
                foreach(var item in projectDto.LandPositionInfos!)
                {
                    var landPosition = _mapper.Map<LandPositionInfo>(item);
                    landPosition.ProjectId = project.ProjectId;
                    //await _unitOfWork.LandPositionInfoRepository.AddAsync(landPosition);
                }
                
            }


            await _unitOfWork.ProjectRepository.AddAsync(project);


            if (!projectDto.Documents.IsNullOrEmpty())
            {

                var documents = await _documentService.CreateDocumentsAsync(projectDto.Documents!);

                foreach(var document in documents)
                {
                    await _documentService.AssignDocumentsToProjectAsync(project.ProjectId, document.DocumentId);
                }
                
            }

            

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

        /// <summary>
        /// This method only used to test export all projects in database
        /// </summary>
        /// <returns></returns>
        public async Task<ExportFileDTO> ExportProjectFileAsync()
        {
            var project = await _unitOfWork.ProjectRepository.GetAllAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Projects");

                int row = 1;

                var properties = typeof(Project).GetProperties();

                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = properties[col].Name;
                }

                foreach (var item in project)
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

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProjectReadDTO>(project);
        }

        public async Task<ProjectReadDTO> CreateProjectDocumentsAsync(string projectId, IEnumerable<DocumentWriteDTO> documentDtos)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId);

            if (project == null) throw new EntityWithIDNotFoundException<Project>(projectId);


            if (!documentDtos.IsNullOrEmpty())
            {

                var documents =await _documentService.CreateDocumentsAsync(documentDtos);

                foreach (var document in documents)
                {
                    await _documentService.AssignDocumentsToProjectAsync(project.ProjectId, document.DocumentId);
                }

            }

            return _mapper.Map<ProjectReadDTO>(project);

        }

        public async Task<ProjectReadDTO> GetProjectOfOwnerAsync(string ownerId)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectsOfOwnerAsync(ownerId);

            return _mapper.Map<ProjectReadDTO>(project);
        }
    }
}
