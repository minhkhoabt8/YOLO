using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                var documents = await _documentService.CreateDocumentsAsync(documentDtos);

                foreach (var document in documents)
                {
                    await _documentService.AssignDocumentsToResettlementProjectAsync(resettlement.ResettlementProjectId, document.DocumentId);
                }

            }

            return _mapper.Map<ResettlementProjectReadDTO>(resettlement);
        }

    }
}
