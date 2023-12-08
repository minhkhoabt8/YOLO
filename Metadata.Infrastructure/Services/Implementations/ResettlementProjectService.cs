using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using Document = Metadata.Core.Entities.Document;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class ResettlementProjectService : IResettlementProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IDocumentService _documentService;

        public ResettlementProjectService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IUploadFileService uploadFileService, IDocumentService documentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _uploadFileService = uploadFileService;
            _documentService = documentService;
        }


        public async Task<ResettlementProjectReadDTO> CreateResettlementProjectAsync(ResettlementProjectWriteDTO dto)
        {

            var resettlement = _mapper.Map<ResettlementProject>(dto);

            resettlement.LastPersonEdit = _userContextService.Username! ??
                throw new CanNotAssignUserException();
            resettlement.LastDateEdit = DateTime.Now.SetKindUtc();

            await _unitOfWork.ResettlementProjectRepository.AddAsync(resettlement);


            if (!dto.ResettlementDocuments.IsNullOrEmpty())
            {
                foreach (var documentDto in dto.ResettlementDocuments!)
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
                        var currResettlementDocument = ResettlementDocument.CreateResettlementDocument(resettlement.ResettlementProjectId, existDocument.DocumentId);

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

                        document.FileSize = documentDto.FileAttach.Length;

                        document.CreatedBy = _userContextService.Username! ??
                            throw new CanNotAssignUserException();

                        var resettlementDocument = ResettlementDocument.CreateResettlementDocument(resettlement.ResettlementProjectId, document.DocumentId);

                        await _unitOfWork.ResettlementDocumentRepository.AddAsync(resettlementDocument);
                    }

                }
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ResettlementProjectReadDTO>(resettlement);
        }

        public Task<IEnumerable<ResettlementProjectReadDTO>> CreateResettlementProjectsAsync(IEnumerable<ResettlementProjectWriteDTO> dto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteResettlementProjectAsync(string id)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.FindAsync(id);

            if (resettlement == null) throw new EntityWithIDNotFoundException<ResettlementProject>(id);

            resettlement.IsDeleted = true;

            resettlement.LastPersonEdit = _userContextService.Username! ??
               throw new CanNotAssignUserException();

            resettlement.LastDateEdit = DateTime.Now.SetKindUtc();

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<ResettlementProjectReadDTO>> GetAllResettlementProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResettlementProjectReadDTO> GetResettlementProjectAsync(string id)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.FindAsync(id, include: "Projects");

            var result = _mapper.Map<ResettlementProjectReadDTO>(resettlement);

            result.ResettlementDocuments =  _mapper.Map<IEnumerable<DocumentReadDTO>>( await _unitOfWork.DocumentRepository.GetDocumentsOfResettlemtProjectAsync(result.ResettlementProjectId!));
            
            return result;
        }

        public async Task<PaginatedResponse<ResettlementProjectReadDTO>> ResettlementProjectQueryAsync(ResettlementProjectQuery query)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.QueryAsync(query);

            return PaginatedResponse<ResettlementProjectReadDTO>.FromEnumerableWithMapping(resettlement, query, _mapper);
        }

        public async Task<ResettlementProjectReadDTO> UpdateResettlementProjectAsync(string id, ResettlementProjectWriteDTO dto)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.FindAsync(id);

            if (resettlement == null)
            {
                throw new EntityWithIDNotFoundException<ResettlementProject>(id);
            }

            if (dto.Code!.ToLower() != resettlement.Code.ToLower() && dto.Name!.ToLower() != resettlement.Name.ToLower())
            {
                var duplicateResettlement = await _unitOfWork.ResettlementProjectRepository.CheckDuplicateResettlementProjectAsync(dto.Code, dto.Name);

                if (duplicateResettlement != null)
                {
                    throw new UniqueConstraintException("Có một dự án tái định cư khác đã tồn tại trong hệ thống");
                }
            }
            
            _mapper.Map(dto, resettlement);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ResettlementProjectReadDTO>(resettlement);
        }

        public async Task<ResettlementProjectReadDTO> GetResettlementProjectByProjectIdAsync(string projectId)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.GetResettlementProjectInProjectAsync(projectId);
            
            var result = _mapper.Map<ResettlementProjectReadDTO>(resettlement);

            result.ResettlementDocuments = _mapper.Map<IEnumerable<DocumentReadDTO>>(await _unitOfWork.DocumentRepository.GetDocumentsOfResettlemtProjectAsync(result.ResettlementProjectId!));

            return result;
        }

        public async Task<ResettlementProjectReadDTO> CreateResettlementProjectDocumentsAsync(string resettlementId, IEnumerable<DocumentWriteDTO> documentDtos)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.FindAsync(resettlementId);

            if (resettlement == null) throw new EntityWithIDNotFoundException<ResettlementProject>(resettlementId);


            if (!documentDtos.IsNullOrEmpty())
            {

                foreach (var documentDto in documentDtos!)
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
                        var currResettlementDocument = ResettlementDocument.CreateResettlementDocument(resettlement.ResettlementProjectId, existDocument.DocumentId);

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

                        document.FileSize = documentDto.FileAttach.Length;

                        document.CreatedBy = _userContextService.Username! ??
                            throw new CanNotAssignUserException();

                        await _documentService.AssignDocumentsToResettlementProjectAsync(resettlement.ResettlementProjectId, document.DocumentId);
                    }

                }

            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ResettlementProjectReadDTO>(resettlement);
        }

        //CheckNameResettlementProjectNotDuplicate
        public async Task<bool> CheckNameResettlementProjectNotDuplicateAsync(string name)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.FindByNameAndIsDeletedStatus(name , false);

            if (resettlement != null && resettlement.Name == name)
            {
                throw new UniqueConstraintException<ResettlementProject>(nameof(resettlement.Name), name);
            }
            return true;
        }

        public async Task<bool> CheckCodeResettlementProjectNotDuplicateAsync(string code)
        {
            var resettlement = await _unitOfWork.ResettlementProjectRepository.FindByCodeAndIsDeletedStatus(code, false);

            if (resettlement != null && resettlement.Code == code)
            {
                throw new UniqueConstraintException<ResettlementProject>(nameof(resettlement.Code), code);
            }
            return true;
        }

    }
}
