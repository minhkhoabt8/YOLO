using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Reflection.Metadata;

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

        public async Task<IEnumerable<DocumentReadDTO>> CreateDocumentsAsync(IEnumerable<DocumentWriteDTO> documentDtos)
        {
            var documentList = new List<Core.Entities.Document>();

           foreach(var documentDto in documentDtos)
           {
                var fileUpload = new UploadFileDTO
                {
                    File = documentDto.FileAttach!,
                    FileName = $"{documentDto.Number}-{documentDto.Notation}-{Guid.NewGuid()}"
                };

                var returnUrl = await _uploadFileService.UploadFileAsync(fileUpload);

                var document = _mapper.Map<Core.Entities.Document>(documentDto);

                document.ReferenceLink = returnUrl;

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
                FileName = $"{documentDto.Number}-{documentDto.Notation}-{Guid.NewGuid()}"
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
    }
}
