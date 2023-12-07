using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using Document = Metadata.Core.Entities.Document;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IDocumentService _documentService;
        private readonly IAuthService _authService;
        private readonly IUploadFileService _uploadFileService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IDocumentService documentService, IAuthService authService, IUploadFileService uploadFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _documentService = documentService;
            _authService = authService;
            _uploadFileService = uploadFileService;
        }

        //api check not allow duplicate project name 
        public async Task<bool> CheckDuplicateProjectNameAsync(string projectName)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectByNameAsync(projectName);
            if (project != null)
            {
                throw new UniqueConstraintException<Project>(nameof(project.ProjectName), projectName);
            }
            return true;
        }
        //api check not allow duplicate project code
        public async Task<bool> CheckDuplicateProjectCodeAsync(string projectCode)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectByProjectCodeAsync(projectCode);
            if (project != null)
            {
                throw new UniqueConstraintException<Project>(nameof(project.ProjectCode), projectCode);
            }
            return true;
        }

        public async Task<ProjectReadDTO> CreateProjectAsync(ProjectWriteDTO projectDto)
        {
            var project = new Project
            {
                ProjectCode = projectDto.ProjectCode,
                ProjectName = projectDto.ProjectName,
                ProjectLocation = projectDto.ProjectLocation,
                Province = projectDto.Province,
                District = projectDto.District,
                Ward = projectDto.Ward,
                ProjectExpense = projectDto.ProjectExpense,
                ProjectApprovalDate = projectDto.ProjectApprovalDate,
                ProjectCreatedTime = projectDto.ProjectCreatedTime,
                ImplementationYear = projectDto.ImplementationYear,
                RegulatedUnitPrice = projectDto.RegulatedUnitPrice,
                ProjectBriefNumber = projectDto.ProjectBriefNumber,
                ProjectNote = projectDto.ProjectNote,
                PriceAppliedCodeId = projectDto.PriceAppliedCodeId,
                ResettlementProjectId = projectDto.ResettlementProjectId,
                CheckCode = projectDto.CheckCode,
                ReportSignal = projectDto.ReportSignal,
                ReportNumber = projectDto.ReportNumber,
                PriceBasis = projectDto.PriceBasis,
                LandCompensationBasis = projectDto.LandCompensationBasis,
                AssetCompensationBasis = projectDto.AssetCompensationBasis,
                ProjectStatus = projectDto.ProjectStatus.ToString(),

            };

            project.ProjectCreatedBy = _userContextService.Username! ??
                throw new CanNotAssignUserException();


            //valid check duplicate project name and project code
            var projectNameDuplicate = await _unitOfWork.ProjectRepository.GetProjectByNameAsync(project.ProjectName);
            if (projectNameDuplicate != null)
            {
                throw new UniqueConstraintException<Project>(projectNameDuplicate.ProjectName ,project.ProjectName);
            }
            var projectCodeDuplicate = await _unitOfWork.ProjectRepository.GetProjectByProjectCodeAsync(project.ProjectCode);
            if(projectCodeDuplicate != null)
            {
                throw new UniqueConstraintException<Project>(projectCodeDuplicate.ProjectCode , project.ProjectCode);
            }

            await _unitOfWork.ProjectRepository.AddAsync(project);

            if (!projectDto.LandPositionInfos.IsNullOrEmpty())
            {
                foreach(var item in projectDto.LandPositionInfos!)
                {
                    var landPosition = _mapper.Map<LandPositionInfo>(item);
                    landPosition.ProjectId = project.ProjectId;
                    await _unitOfWork.LandPositionInfoRepository.AddAsync(landPosition);
                }
            }

            if(projectDto.ResettlementProject != null )
            {
                var projectResetlement = new ResettlementProject
                {
                    Code = projectDto.ResettlementProject.Code!,
                    Name = projectDto.ResettlementProject.Name!,
                    LimitToResettlement = projectDto.ResettlementProject.LimitToResettlement,
                    LimitToConsideration = projectDto.ResettlementProject.LimitToConsideration,
                    Position = projectDto.ResettlementProject.Position,
                    LandNumber = projectDto.ResettlementProject.LandNumber,
                    ImplementYear = projectDto.ResettlementProject.ImplementYear,
                    LandPrice = projectDto.ResettlementProject.LandPrice,
                    Note = projectDto.ResettlementProject.Note
                };
                

                await _unitOfWork.ResettlementProjectRepository.AddAsync(projectResetlement);

                project.ResettlementProjectId = projectResetlement.ResettlementProjectId;

                projectResetlement.LastPersonEdit = _userContextService.Username! ??
                            throw new CanNotAssignUserException();

                if (!projectDto.ResettlementProject.ResettlementDocuments.IsNullOrEmpty())
                {

                    foreach (var documentDto in projectDto.ResettlementProject.ResettlementDocuments!)
                    {
                        //If documentDTO.Id not null => assign exist document to new project
                        if (!documentDto.Id.IsNullOrEmpty())
                        {
                            //check document exist
                            var existDocument = await _unitOfWork.DocumentRepository.FindAsync(documentDto.Id!);

                            if (existDocument == null || existDocument.IsDeleted)
                            {
                                throw new EntityWithIDNotFoundException<Document>(documentDto.Id!);
                            }

                            //Assign Document To Project
                            var currResettlementDocument = ResettlementDocument.CreateResettlementDocument(projectResetlement.ResettlementProjectId, existDocument.DocumentId);

                            await _unitOfWork.ResettlementDocumentRepository.AddAsync(currResettlementDocument);
                        }
                        else
                        {
                            var fileUpload = new UploadFileDTO
                            {
                                File = documentDto.FileAttach!,
                                FileName = $"{documentDto.Number}-{documentDto.Notation}-{Guid.NewGuid()}",
                                FileType = FileTypeExtensions.ToFileMimeTypeString(documentDto.FileType),
                            };

                            var returnUrl = await _uploadFileService.UploadFileAsync(fileUpload);

                            var document = _mapper.Map<Core.Entities.Document>(documentDto);

                            document.ReferenceLink = returnUrl;

                            document.FileName = documentDto.FileName!;

                            document.FileSize = documentDto.FileAttach!.Length;

                            document.CreatedBy = _userContextService.Username! ??
                                throw new CanNotAssignUserException();

                            await _unitOfWork.DocumentRepository.AddAsync(document);

                            var resettlementDocument = ResettlementDocument.CreateResettlementDocument(projectResetlement.ResettlementProjectId, document.DocumentId);

                            await _unitOfWork.ResettlementDocumentRepository.AddAsync(resettlementDocument);
                        }

                    }
                }
            }

            if (!projectDto.ProjectDocuments.IsNullOrEmpty())
            {

                foreach(var documentDto  in projectDto.ProjectDocuments!)
                {
                    //If documentDTO.Id not null => assign exist document to new project
                    if (!documentDto.Id.IsNullOrEmpty())
                    {
                        //check document exist
                        var existDocument = await _unitOfWork.DocumentRepository.FindAsync(documentDto.Id!);

                        if(existDocument == null || existDocument.IsDeleted)
                        {
                            throw new EntityWithIDNotFoundException<Document>(documentDto.Id!);
                        }
                            
                        //Assign Document To Project
                        var projectDocument = ProjectDocument.CreateProjectDocument(project.ProjectId, existDocument.DocumentId);

                        await _unitOfWork.ProjectDocumentRepository.AddAsync(projectDocument);
                    }
                    //Else documentDTO.Id null => create new Document with New Project
                    else
                    {
                        var fileUpload = new UploadFileDTO
                        {
                            File = documentDto.FileAttach!,
                            FileName = $"{documentDto.Number}-{documentDto.Notation}-{Guid.NewGuid()}",
                            FileType = FileTypeExtensions.ToFileMimeTypeString(documentDto.FileType),
                        };

                        var returnUrl = await _uploadFileService.UploadFileAsync(fileUpload);

                        var document = _mapper.Map<Core.Entities.Document>(documentDto);

                        document.ReferenceLink = returnUrl;

                        document.FileName = documentDto.FileName!;

                        document.FileSize = documentDto.FileAttach!.Length;

                        document.CreatedBy = _userContextService.Username! ??
                            throw new CanNotAssignUserException();

                        await _unitOfWork.DocumentRepository.AddAsync(document);

                        var projectDocument = ProjectDocument.CreateProjectDocument(project.ProjectId, document.DocumentId);

                        await _unitOfWork.ProjectDocumentRepository.AddAsync(projectDocument);
                    }

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
                .FindAsync(projectId, include: "LandPositionInfos, Owners, Plans, PriceAppliedCode, UnitPriceLands, ProjectDocuments, ResettlementProject");
            
            var projectReadDto =  _mapper.Map<ProjectReadDTO>(project);

            if(projectReadDto.ResettlementProject != null)
            {
                //set to null to avoid model mapping again
                projectReadDto.ResettlementProject.Projects = null;
            }

            var projectDocuments = await _unitOfWork.DocumentRepository.GetDocumentsOfProjectAsync(projectId);

            projectReadDto.ProjectDocuments = _mapper.Map<IEnumerable<DocumentReadDTO>>(projectDocuments);

            if(!projectReadDto.ResettlementProjectId.IsNullOrEmpty() || projectReadDto.ResettlementProject != null)
            {
                //Attach Resettlement Document
                var resettlementProjectDocuments = await _unitOfWork.DocumentRepository.GetDocumentsOfResettlemtProjectAsync(projectReadDto.ResettlementProjectId!);

                projectReadDto.ResettlementProject!.ResettlementDocuments = _mapper.Map<IEnumerable<DocumentReadDTO>>(resettlementProjectDocuments);
            }

            return projectReadDto;
        }

        public async Task<PaginatedResponse<ProjectReadDTO>> ProjectQueryAsync(ProjectQuery query)
        {
            var projects = await _unitOfWork.ProjectRepository.QueryAsync(query);

            return PaginatedResponse<ProjectReadDTO>.FromEnumerableWithMapping(projects, query, _mapper);
        }

        public async Task<ProjectReadDTO> UpdateProjectAsync(string projectId, ProjectWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(projectId, include: "Owners");

            if(project == null) throw new EntityWithIDNotFoundException<Project>(projectId);

            //Project that have owners cannot update the price apply code

            //if(project.PriceAppliedCodeId != dto.PriceAppliedCodeId)
            //{
            //    if(project.Owners.Count > 0)
            //    {
            //        throw new InvalidActionException("Cannot Update Price Apply Code In Project That Aldready Have Owners");
            //    }
            //}

            if(dto.ProjectCode.ToLower() != project.ProjectCode.ToLower() && dto.ProjectName.ToLower() != project.ProjectName.ToLower())
            {
                var duplicateProject = await _unitOfWork.ProjectRepository.CheckDuplicateProjectAsync(dto.ProjectCode, dto.ProjectName);

                if (duplicateProject != null)
                {
                    throw new UniqueConstraintException("Có một dự án khác đã tồn tại trong hệ thống");
                }
            }

            if(project.Owners != null)
            {
                if(project.PriceAppliedCodeId != dto.PriceAppliedCodeId)
                {
                    throw new InvalidActionException("Không thể cập nhật mã áp giá của dự án đã có chủ sở hữu.");
                }
            }

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

                foreach(var documentDto in documentDtos)
                {
                    //If documentDTO.Id not null => assign exist document to new project
                    if (!documentDto.Id.IsNullOrEmpty())
                    {
                        //check document exist
                        var existDocument = await _unitOfWork.DocumentRepository.FindAsync(documentDto.Id!);

                        if (existDocument == null || existDocument.IsDeleted)
                        {
                            throw new EntityWithIDNotFoundException<Document>(documentDto.Id!);
                        }

                        //Assign Document To Project
                        var currProjectDocument = ProjectDocument.CreateProjectDocument(project.ProjectId, existDocument.DocumentId);

                        await _unitOfWork.ProjectDocumentRepository.AddAsync(currProjectDocument);
                    }
                    else
                    {
                        var fileUpload = new UploadFileDTO
                        {
                            File = documentDto.FileAttach!,
                            FileName = $"{documentDto.Number}-{documentDto.Notation}-{Guid.NewGuid()}",
                            FileType = FileTypeExtensions.ToFileMimeTypeString(documentDto.FileType),
                        };

                        var returnUrl = await _uploadFileService.UploadFileAsync(fileUpload);

                        var document = _mapper.Map<Core.Entities.Document>(documentDto);

                        document.ReferenceLink = returnUrl;

                        document.FileName = documentDto.FileName!;

                        document.FileSize = documentDto.FileAttach!.Length;

                        document.CreatedBy = _userContextService.Username! ??
                            throw new CanNotAssignUserException();

                        await _unitOfWork.DocumentRepository.AddAsync(document);

                        var projectDocument = ProjectDocument.CreateProjectDocument(project.ProjectId, document.DocumentId);

                        await _unitOfWork.ProjectDocumentRepository.AddAsync(projectDocument);
                    }
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
