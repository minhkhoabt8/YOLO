using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Reflection.Metadata;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<DocumentReadDTO>> AssignDocumentsToProjectAsync(string projectId, IEnumerable<DocumentWriteDTO> documentDtos)
        {

            var documents = _mapper.Map<IEnumerable<Core.Entities.Document>>(documentDtos);

            foreach (var document in documents)
            {
                document.CreatedBy = _userContextService.Username! ?? throw new CanNotAssignUserException();

                var projectDocument = ProjectDocument.CreateProjectDocument(projectId, document.DocumentId);

                await _unitOfWork.ProjectDocumentRepository.AddAsync(projectDocument);
            }

            //TODO:Upload to WS3

            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<DocumentReadDTO>>(documents);

        }
    }
}
