using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IUploadFileService _uploadFileService;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IUploadFileService uploadFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _uploadFileService = uploadFileService;
        }

        public async Task AssignDocumentsToProjectAsync(string projectId, string documentId)
        {

            var projectDocument = ProjectDocument.CreateProjectDocument(projectId, documentId);

            await _unitOfWork.ProjectDocumentRepository.AddAsync(projectDocument);

            await _unitOfWork.CommitAsync();

        }

        public async Task<ExportFileDTO> GetFileImportExcelTemplateAsync(string name)
        {
            string fileName = "";

            if (name.ToLower().Equals("owner"))
            {
                fileName = GetFileTemplateDirectory.Get("BangNhapChuSoHuu");
            }

            if (!File.Exists(fileName))
            {
                throw new EntityWithAttributeNotFoundException<ExportFileDTO>(nameof(ExportFileDTO.FileName), name);
            }

            return new ExportFileDTO
            {
                FileName = Path.GetFileName(fileName) + DateTime.Now.SetKindUtc(),
                FileByte = File.ReadAllBytes(fileName),
                FileType = FileTypeExtensions.ToFileMimeTypeString(FileTypeEnum.xlsx)
            };
        }
        

        public async Task<IEnumerable<DocumentReadDTO>> CreateDocumentsAsync(IEnumerable<DocumentWriteDTO> documentDtos)
        {
            var documentList = new List<Core.Entities.Document>();

           foreach(var documentDto in documentDtos)
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

                await _unitOfWork.DocumentRepository.AddAsync(document);

                documentList.Add(document);

           }
           
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<DocumentReadDTO>>(documentList);
        }

        public async Task<DocumentReadDTO> CreateDocumentAsync(DocumentWriteDTO documentDto)
        {
            var fileUpload = new UploadFileDTO
            {
                File = documentDto.FileAttach!,
                FileName = $"{documentDto.Number}-{documentDto.Notation}-{Guid.NewGuid()}",
                FileType = FileTypeExtensions.ToFileMimeTypeString(documentDto.FileType)
            };

            var returnUrl = await _uploadFileService.UploadFileAsync(fileUpload);

            var document = _mapper.Map<Core.Entities.Document>(documentDto);

            document.ReferenceLink = returnUrl;

            document.CreatedBy = _userContextService.Username! ??
                throw new CanNotAssignUserException();

            await _unitOfWork.DocumentRepository.AddAsync(document);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<DocumentReadDTO>(document);
        }

        public async Task DeleteDocumentAsync(string documentId)
        {
            var document = await _unitOfWork.DocumentRepository.FindAsync(documentId);

            if (document == null) throw new EntityWithIDNotFoundException<Core.Entities.Document>(documentId);

            document.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<DocumentReadDTO> UpdateDocumentAsync(string documentId, DocumentWriteDTO dto)
        {
            var document = await _unitOfWork.DocumentRepository.FindAsync(documentId);

            if (document == null) throw new EntityWithIDNotFoundException<Core.Entities.Document>(documentId);

            if (!dto.FileAttach.IsNullOrEmpty())
            {
                var fileUpload = new UploadFileDTO
                {
                    File = dto.FileAttach!,
                    FileName = $"{dto.Number}-{dto.Notation}-{Guid.NewGuid()}",
                    FileType = FileTypeExtensions.ToFileMimeTypeString(dto.FileType)
                };

                document.ReferenceLink =  await _uploadFileService.UploadFileAsync(fileUpload);
            }

            _mapper.Map(dto, document);

            document.CreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<DocumentReadDTO>(document);
        }

        public async Task<PaginatedResponse<DocumentReadDTO>> QueryDocumentAsync(DocumentQuery query)
        {
            var owner = await _unitOfWork.DocumentRepository.QueryAsync(query);
            return PaginatedResponse<DocumentReadDTO>.FromEnumerableWithMapping(owner, query, _mapper);
        }

        public async Task<IEnumerable<DocumentReadDTO>> GetDocumentsOfProjectAsync(string projectId)
        {
            var documents = await _unitOfWork.DocumentRepository.GetDocumentsOfProjectAsync(projectId);

            return _mapper.Map<IEnumerable<DocumentReadDTO>>(documents);
        }

        public async Task AssignDocumentsToResettlementProjectAsync(string resettlementProjectId, string documentId)
        {
            var resettlementDocument = ResettlementDocument.CreateResettlementDocument(resettlementProjectId, documentId);

            await _unitOfWork.ResettlementDocumentRepository.AddAsync(resettlementDocument);

            await _unitOfWork.CommitAsync();
        }
    }
}
