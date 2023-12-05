using Amazon.S3.Model;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class PriceAppliedCodeService : IPriceAppliedCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUserContextService _userContextService;

        public PriceAppliedCodeService(IUnitOfWork unitOfWork, IMapper mapper, IUploadFileService uploadFileService, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadFileService = uploadFileService;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<PriceAppliedCodeReadDTO>> CreatePriceAppliedCodesAsync(IEnumerable<PriceAppliedCodeWriteDTO> dto)
        {
            var priceAppliedCodes = new List<PriceAppliedCodeReadDTO>();

            foreach (var item in dto)
            {
                var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(item.UnitPriceCode);

                if (existPriceAppliedCode != null)
                {
                    throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), item.UnitPriceCode);
                }


                var priceAppliedCode = _mapper.Map<PriceAppliedCode>(item);

                if (!item.UnitPriceAssets.IsNullOrEmpty())
                {
                    // Map and set the PriceAppliedCodeId for each UnitPriceAsset
                    foreach (var asset in item.UnitPriceAssets)
                    {
                        var assetDto = _mapper.Map<UnitPriceAsset>(asset);

                        assetDto.PriceAppliedCodeId = priceAppliedCode.PriceAppliedCodeId;

                        priceAppliedCode.UnitPriceAssets.Add(assetDto);
                    }
                }

                if (!item.Documents.IsNullOrEmpty())
                {
                    foreach (var documentDto in item.Documents!)
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
                            var currResettlementDocument = PriceAppliedCodeDocument.CreatePriceAppliedCodeDocument(priceAppliedCode.PriceAppliedCodeId, existDocument.DocumentId);

                            await _unitOfWork.PriceAppliedCodeDocumentRepository.AddAsync(currResettlementDocument);
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

                            await _unitOfWork.DocumentRepository.AddAsync(document);

                            var priceAppliedCodeDocument = PriceAppliedCodeDocument.CreatePriceAppliedCodeDocument(priceAppliedCode.PriceAppliedCodeId, document.DocumentId);

                            await _unitOfWork.PriceAppliedCodeDocumentRepository.AddAsync(priceAppliedCodeDocument);

                        }

                    }
                }

                //await _unitOfWork.PriceAppliedCodeRepository.AddAsync(priceAppliedCode);

                var priceAppliedCodeReadDTO = _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);

                priceAppliedCodes.Add(priceAppliedCodeReadDTO);
            }

            await _unitOfWork.CommitAsync();

            return priceAppliedCodes;
        }

        public async Task<PriceAppliedCodeReadDTO> CreatePriceAppliedCodeAsync(PriceAppliedCodeWriteDTO dto)
        {
            var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(dto.UnitPriceCode);

            if (existPriceAppliedCode != null)
            {
                throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), dto.UnitPriceCode);
            }


            var priceAppliedCode = _mapper.Map<PriceAppliedCode>(dto);

            if (!dto.UnitPriceAssets.IsNullOrEmpty())
            {
                // Map and set the PriceAppliedCodeId for each UnitPriceAsset
                foreach (var asset in dto.UnitPriceAssets)
                {
                    //var assetDto = _mapper.Map<UnitPriceAsset>(asset);
                    asset.PriceAppliedCodeId = priceAppliedCode.PriceAppliedCodeId;
                    //priceAppliedCode.UnitPriceAssets.Add(assetDto);
                }
            }

            await _unitOfWork.PriceAppliedCodeRepository.AddAsync(priceAppliedCode);

            if (!dto.Documents.IsNullOrEmpty())
            {
                foreach(var documentDto in dto.Documents!)
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
                        var currResettlementDocument = PriceAppliedCodeDocument.CreatePriceAppliedCodeDocument(priceAppliedCode.PriceAppliedCodeId, existDocument.DocumentId);

                        await _unitOfWork.PriceAppliedCodeDocumentRepository.AddAsync(currResettlementDocument);
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

                        await _unitOfWork.DocumentRepository.AddAsync(document);

                        var priceAppliedCodeDocument = PriceAppliedCodeDocument.CreatePriceAppliedCodeDocument(priceAppliedCode.PriceAppliedCodeId, document.DocumentId);

                        await _unitOfWork.PriceAppliedCodeDocumentRepository.AddAsync(priceAppliedCodeDocument);
                    }
                    
                }
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);
        }

        public async Task DeletePriceAppliedCodeAsync(string Id)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(Id, include: "UnitPriceAssets, Projects");

            if (priceAppliedCode == null)
            {
                throw new EntityWithIDNotFoundException<PriceAppliedCode>(Id);
            }

            //Cannot Delete Price Applied Code that existed in Project
            if (priceAppliedCode.Projects.Count() > 0)
            {
                throw new InvalidActionException();
            }

            
            foreach(var asset in priceAppliedCode.UnitPriceAssets) 
            {
                _unitOfWork.UnitPriceAssetRepository.Delete(asset);
            }

            priceAppliedCode.IsDeleted = true;

            _unitOfWork.PriceAppliedCodeRepository.Delete(priceAppliedCode);

            await _unitOfWork.CommitAsync();
        }

        public async Task<PriceAppliedCodeReadDTO> GetPriceAppliedCodeAsync(string Id)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(Id, include: "UnitPriceAssets, PriceAppliedCodeDocuments");

            var priceAppliedCodeDTO =  _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);

            priceAppliedCodeDTO.Documents = _mapper.Map<IEnumerable<DocumentReadDTO>>(await _unitOfWork.DocumentRepository.GetDocumentsOfPriceAppliedCodeAsync(priceAppliedCode.PriceAppliedCodeId));

            return priceAppliedCodeDTO;
        }

        public async Task<PaginatedResponse<PriceAppliedCodeReadDTO>> QueryPriceAppliedCodeAsync(PriceAppliedCodeQuery paginationQuery)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.QueryAsync(paginationQuery);

            return PaginatedResponse<PriceAppliedCodeReadDTO>.FromEnumerableWithMapping(priceAppliedCode, paginationQuery, _mapper);
        }

        public async Task<PriceAppliedCodeReadDTO> UpdatePriceAppliedCodeAsync(string Id, PriceAppliedCodeWriteDTO dto)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(Id)
                ??throw new EntityWithIDNotFoundException<PriceAppliedCode>(Id);

            if(dto.UnitPriceCode.ToLower() != priceAppliedCode.UnitPriceCode.ToLower())
            {
                var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(dto.UnitPriceCode);

                if (existPriceAppliedCode != null)
                    throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), dto.UnitPriceCode);
            }
            
            _mapper.Map(dto, priceAppliedCode);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);
        }

        public async Task<PriceAppliedCodeReadDTO> CheckDuplicateCodeAsync(string code)
        {
            var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(code);
            if (existPriceAppliedCode != null)
                throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), code);
            return _mapper.Map<PriceAppliedCodeReadDTO>(existPriceAppliedCode);
        }

        public async Task<PriceAppliedCodeReadDTO> CreatePriceAplliedDocumentsAsync(string priceAppliedCodeId, IEnumerable<DocumentWriteDTO> documentDtos)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(priceAppliedCodeId);

            if (priceAppliedCode == null) throw new EntityWithIDNotFoundException<PriceAppliedCode>(priceAppliedCodeId);


            if (!documentDtos.IsNullOrEmpty())
            {

                foreach (var documentDto in documentDtos)
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
                        var currPriceAppliedCodeDocument = PriceAppliedCodeDocument.CreatePriceAppliedCodeDocument(priceAppliedCode.PriceAppliedCodeId, existDocument.DocumentId);

                        await _unitOfWork.PriceAppliedCodeDocumentRepository.AddAsync(currPriceAppliedCodeDocument);
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

                        var currPriceAppliedCodeDocument = PriceAppliedCodeDocument.CreatePriceAppliedCodeDocument(priceAppliedCode.PriceAppliedCodeId, document.DocumentId);

                        await _unitOfWork.PriceAppliedCodeDocumentRepository.AddAsync(currPriceAppliedCodeDocument);
                    }
                }
            }

            return _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);

        }
    }
}
