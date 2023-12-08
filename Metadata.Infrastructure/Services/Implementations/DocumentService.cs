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
        private readonly IGetFileTemplateDirectory _getFileTemplateDirectory;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IUploadFileService uploadFileService, IGetFileTemplateDirectory getFileTemplateDirectory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _uploadFileService = uploadFileService;
            _getFileTemplateDirectory = getFileTemplateDirectory;
        }

        public async Task AssignDocumentsToProjectAsync(string projectId, string documentId)
        {

            var projectDocument = ProjectDocument.CreateProjectDocument(projectId, documentId);

            await _unitOfWork.ProjectDocumentRepository.AddAsync(projectDocument);

            await _unitOfWork.CommitAsync();

        }
        /// <summary>
        /// Map user file name input to system file name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="EntityWithAttributeNotFoundException{ExportFileDTO}"></exception>
        public async Task<ExportFileDTO> GetFileImportExcelTemplateAsync(string name)
        {
            string fileName = "";

            if (name.ToLower().Equals("owner"))
            {
                fileName = _getFileTemplateDirectory.GetImport("OwnerImportTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("assetgroup"))
            {
                fileName = _getFileTemplateDirectory.GetImport("AssetGroupTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("assetunit"))
            {
                fileName = _getFileTemplateDirectory.GetImport("AssetUnitTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("deductiontype"))
            {
                fileName = _getFileTemplateDirectory.GetImport("DeductionTypeTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("documenttype"))
            {
                fileName = _getFileTemplateDirectory.GetImport("DocumentType");
            }

            if (name.ToLower().Replace(" ", "").Equals("landgroup"))
            {
                fileName = _getFileTemplateDirectory.GetImport("LandGroupTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("landtype"))
            {
                fileName = _getFileTemplateDirectory.GetImport("LandTypeTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("organizationtype"))
            {
                fileName = _getFileTemplateDirectory.GetImport("OrganizationTypeTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("supporttype"))
            {
                fileName = _getFileTemplateDirectory.GetImport("SupportTypeTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("unitpriceland"))
            {
                fileName = _getFileTemplateDirectory.GetImport("UnitPriceLandTemplate");
            }

            if (name.ToLower().Replace(" ", "").Equals("unitpriceasset"))
            {
                fileName = _getFileTemplateDirectory.GetImport("UnitPriceAssetTemplate");
            }

            if (!File.Exists(fileName))
            {
                throw new EntityWithAttributeNotFoundException<ExportFileDTO>(nameof(ExportFileDTO.FileName), name);
            }

            return new ExportFileDTO
            {
                FileName = Path.GetFileName(fileName),
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

            document.FileSize = documentDto.FileAttach.Length;

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

            var projectDocuments = await _unitOfWork.ProjectDocumentRepository.FindByDocumentIdAsync(documentId);
            
            if (projectDocuments != null)
            {
                foreach (var projectDoc in projectDocuments)
                {
                    _unitOfWork.ProjectDocumentRepository.Delete(projectDoc);
                }
            }

            var resettlementDocument = await _unitOfWork.ResettlementDocumentRepository.FindByDocumentIdAsync(documentId);

            if (resettlementDocument != null)
            {
                foreach(var resettlementDoc in resettlementDocument)
                {
                    _unitOfWork.ResettlementDocumentRepository.Delete(resettlementDoc);
                }
            }


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

        public async Task<DocumentReadDTO> CheckDuplicateDocumentAsync(int number, string notation, string epitome)
        {
            var document = await _unitOfWork.DocumentRepository.CheckDuplicateDocumentAsync(number, notation, epitome);

            return _mapper.Map<DocumentReadDTO>(document);
        }


    }
}
