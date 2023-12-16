using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
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
            await EnsureDocumentTypeCodeNotDuplicate(documentType.Code, documentType.Name);
            var documentTypeEntity = _mapper.Map<DocumentType>(documentType);
            await _unitOfWork.DocumentTypeRepository.AddAsync(documentTypeEntity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DocumentTypeReadDTO>(documentTypeEntity);

        }

        public async Task<IEnumerable<DocumentTypeReadDTO>> CreateListDocumentTypeAsync(IEnumerable<DocumentTypeWriteDTO> documentTypeWrites)
        {            
            var documentTypes = new List<DocumentTypeReadDTO>();
                   foreach (var documentTypeWrite in documentTypeWrites)
            {
                await EnsureDocumentTypeCodeNotDuplicate(documentTypeWrite.Code , documentTypeWrite.Name);
                var documentTypeEntity = _mapper.Map<DocumentType>(documentTypeWrite);
                await _unitOfWork.DocumentTypeRepository.AddAsync(documentTypeEntity);
                await _unitOfWork.CommitAsync();
                var readDTO = _mapper.Map<DocumentTypeReadDTO>(documentTypeEntity);
                documentTypes.Add(readDTO);
            }
            return documentTypes;
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

        public async Task<IEnumerable<DocumentTypeReadDTO>> GetAllActivedDocumentTypes()
        {
            var documentTypes = await _unitOfWork.DocumentTypeRepository.GetAllActivedDocumentTypes();
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
            await EnsureDocumentTypeCodeNotDuplicateForUpdate(documentType.Code, documentType.Name,id);
            _mapper.Map(documentType, documentTypeEntity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DocumentTypeReadDTO>(documentTypeEntity);
        }

        private async Task EnsureDocumentTypeCodeNotDuplicate(string code,string name)
        {
            var documentType = await _unitOfWork.DocumentTypeRepository.FindByCodeAndIsDeletedStatus(code,false);
            if (documentType != null && documentType.Code==code)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentType.Code),code);
            }
            var documentType2 = await _unitOfWork.DocumentTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (documentType2 != null && documentType2.Name == name)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentType2.Name), name);
            }
        }
        public async Task CheckNameDocumentTypeNotDuplicate(string name)
        {
            var documentType = await _unitOfWork.DocumentTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (documentType != null && documentType.Name == name)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentType.Name), name);
            }
            
        }

        public async Task CheckCodeDocumentTypeNotDuplicate(string code)
        {
            var documentType = await _unitOfWork.DocumentTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (documentType != null && documentType.Code == code)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentType.Code), code);
            }
        }

        public async Task<PaginatedResponse<DocumentTypeReadDTO>> QueryDocumentTypeAsync(DocumentTypeQuery paginationQuery)
        {
            var documentTypes = await _unitOfWork.DocumentTypeRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<DocumentTypeReadDTO>.FromEnumerableWithMapping(documentTypes, paginationQuery, _mapper);
        }

        //EnsureDocumentTyoeNotExistForUpdate

        private async Task EnsureDocumentTypeCodeNotDuplicateForUpdate(string code, string name, string id)
        {
            var documentTypeCode = await _unitOfWork.DocumentTypeRepository.FindByCodeAndIsDeletedStatusForUpdate(code, id, false);
            if (documentTypeCode != null && documentTypeCode.Code == code && documentTypeCode.DocumentTypeId != id)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentTypeCode.Code), code);
            }
            var documentTypeName = await _unitOfWork.DocumentTypeRepository.FindByNameAndIsDeletedStatusForUpdate(name, id, false);
            if (documentTypeName != null && documentTypeName.Name == name && documentTypeName.DocumentTypeId != id)
            {
                throw new UniqueConstraintException<DocumentType>(nameof(documentTypeName.Name), name);
            }
        }


        //import data from excel
        public async Task<List<DocumentTypeReadDTO>> ImportDocumenTypeFromExcelAsync(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found", filePath);

            List<DocumentTypeWriteDTO> documentType = new List<DocumentTypeWriteDTO>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.End.Row;

                for (int row = 4; row <= totalRows; row++)
                {
                    string code = worksheet.Cells[row, 1].Text;
                    string name = worksheet.Cells[row, 2].Text;
                    if (string.IsNullOrEmpty(code) ||
                        string.IsNullOrEmpty(name))
                    {

                        continue;
                    }
                    documentType.Add(new DocumentTypeWriteDTO { Code = code, Name = name });
                }
            }

            List<DocumentTypeReadDTO> importedObjects = new List<DocumentTypeReadDTO>();
            foreach (var sp in documentType)
            {
                var importedObject = await CreateDocumentTypeAsync(sp);
                if (importedObject != null)
                {
                    importedObjects.Add(importedObject);
                }
            }
            return importedObjects;
        }

    }
}
