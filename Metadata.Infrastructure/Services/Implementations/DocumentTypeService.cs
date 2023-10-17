using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class DocumentTypeService : IDocumentTypeService
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DocumentTypeReadDTO> CreateDocumentTypeAsync(DocumentTypeWriteDTO documentType)
        {
           /* EnsureDocumentTypeCodeNotDuplicate(documentType.Code);*/
            var documentTypeEntity = _mapper.Map<DocumentType>(documentType);
            await _unitOfWork.DocumentTypeRepository.AddAsync(documentTypeEntity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DocumentTypeReadDTO>(documentTypeEntity);

        }

        public async Task<bool> DeleteDocumentTypeAsync(string id)
        {
            var documentType = await _unitOfWork.DocumentTypeRepository.FindAsync(id);
            if (documentType == null)
            {
                throw new EntityWithIDNotFoundException<DocumentType>(id);
            }
            documentType.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<DocumentTypeReadDTO>> GetAllDocumentTypesAsync()
        {
            var documentTypes = await _unitOfWork.DocumentTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DocumentTypeReadDTO>>(documentTypes);
        }

        public async Task<DocumentTypeReadDTO> GetDocumentTypeAsync(string id)
        {
            var documentType = await _unitOfWork.DocumentTypeRepository.FindAsync(id);
            return _mapper.Map<DocumentTypeReadDTO>(documentType);
        }

        public async Task<DocumentTypeReadDTO> UpdateDocumentTypeAsync(string id, DocumentTypeWriteDTO documentType)
        {
            var documentTypeEntity = await  _unitOfWork.DocumentTypeRepository.FindAsync(id);
            if (documentTypeEntity == null)
            {
                throw new EntityWithIDNotFoundException<DocumentType>(id);
            }
            _mapper.Map(documentType, documentTypeEntity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DocumentTypeReadDTO>(documentTypeEntity);
        }

        private async Task EnsureDocumentTypeCodeNotDuplicate(string code)
        {
            var documentType = await _unitOfWork.DocumentTypeRepository.FindAsync(code);
            if (documentType != null)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentType.Code),code);
            }
        }
    }
}
