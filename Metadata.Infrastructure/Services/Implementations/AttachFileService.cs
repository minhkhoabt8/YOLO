﻿using Amazon.S3.Model;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class AttachFileService : IAttachFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IUploadFileService _uploadFileService;

        public AttachFileService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IUploadFileService uploadFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _uploadFileService = uploadFileService;
        }

        public async Task<IEnumerable<AttachFileReadDTO>> CreateOwnerAttachFilesAsync(string ownerId, IEnumerable<AttachFileWriteDTO> dto)
        {
            //var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            //if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (dto == null) throw new InvalidActionException(nameof(dto));

            var fileList = new List<Core.Entities.AttachFile>();

            foreach (var item in dto)
            {
                
                var fileUpload = new UploadFileDTO
                {
                    File = item.AttachFile!,
                    FileName = $"{item.Name}-{Guid.NewGuid()}",
                    FileType = FileTypeExtensions.ToFileMimeTypeString(item.FileType)
                    
                };

                var file = _mapper.Map<AttachFile>(item);

                file.OwnerId = ownerId;

                file.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

                file.CreatedBy = _userContextService.Username! ??
                    throw new CanNotAssignUserException();

                await _unitOfWork.AttachFileRepository.AddAsync(file);

                await _unitOfWork.CommitAsync();

                fileList.Add(file);

            }

            return _mapper.Map<IEnumerable<AttachFileReadDTO>>(fileList);
        }

        public async Task<AttachFileReadDTO> GetAttachFileDetailsAsync(string id)
        {
            return _mapper.Map<AttachFileReadDTO>(await _unitOfWork.AttachFileRepository.FindAsync(id));
        }

        public async Task<IEnumerable<AttachFileReadDTO>> GetAllAttachFileAsync()
        {
            return _mapper.Map<IEnumerable<AttachFileReadDTO>>(await _unitOfWork.AttachFileRepository.GetAllAsync());
        }


        public async Task<IEnumerable<AttachFileReadDTO>> CreateAttachFilesAsync(IEnumerable<AttachFileWriteDTO> dto)
        {
            var fileList = new List<Core.Entities.AttachFile>();

            foreach (var item in dto)
            {

                var fileUpload = new UploadFileDTO
                {
                    File = item.AttachFile!,
                    FileName = $"{item.Name}-{Guid.NewGuid()}",
                    FileType = FileTypeExtensions.ToFileMimeTypeString(item.FileType)

                };

                var file = _mapper.Map<AttachFile>(item);

                file.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

                file.CreatedBy = _userContextService.Username! ??
                    throw new CanNotAssignUserException();

                await _unitOfWork.AttachFileRepository.AddAsync(file);

                await _unitOfWork.CommitAsync();

                fileList.Add(file);

            }

            return _mapper.Map<IEnumerable<AttachFileReadDTO>>(fileList);
        }

        public async Task DeleteAttachFileAsync(string fileId)
        {
            var file = await _unitOfWork.AttachFileRepository.FindAsync(fileId);

            if (file == null) throw new EntityWithIDNotFoundException<AttachFile>(fileId);

            file.GcnLandInfoId= null;
            file.MeasuredLandInfoId= null;
            file.OwnerId = null;
            file.PlanId = null;

            await _unitOfWork.CommitAsync();
        }

        public async Task<AttachFileReadDTO> UpdateAttachFileAsync(string fileId, AttachFileWriteDTO dto)
        {
            var file = await _unitOfWork.AttachFileRepository.FindAsync(fileId);

            if (file == null) throw new EntityWithIDNotFoundException<AttachFile>(fileId);

            _mapper.Map(dto, file);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AttachFileReadDTO>(file);
        }

        public async Task UploadAttachFileAsync(IEnumerable<AttachFileWriteDTO> files)
        {
            foreach (var item in files)
            {
                
                //file.Name = item.AttachFile.Name;
                //file.FileType = Path.GetExtension(item.AttachFile.Name);

                var fileUpload = new UploadFileDTO
                {
                    File = item.AttachFile!,
                    FileName = $"{item.Name}-{Guid.NewGuid()}",
                    FileType = FileTypeExtensions.ToFileMimeTypeString(item.FileType)
                };

                var file = _mapper.Map<AttachFile>(item);

                var returnUrl = await _uploadFileService.UploadFileAsync(fileUpload);

                file.ReferenceLink = returnUrl;

                file.CreatedBy = _userContextService.Username! ??
                    throw new CanNotAssignUserException();

                await _unitOfWork.AttachFileRepository.AddAsync(file);
            }
        }

        public async Task<AttachFileReadDTO> UploadSignedPdfAttachFileAsync(AttachFileWriteDTO file)
        {
            var fileUpload = new UploadFileDTO
            {
                File = file.AttachFile!,
                FileName = $"{file.Name}-{Guid.NewGuid()}",
                FileType = FileTypeExtensions.ToFileMimeTypeString(file.FileType)
            };

            var attachFile = _mapper.Map<AttachFile>(file);

            attachFile.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

            attachFile.CreatedBy = _userContextService.Username! ??
                throw new CanNotAssignUserException();

            await _unitOfWork.AttachFileRepository.AddAsync(attachFile);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AttachFileReadDTO>(attachFile);
        }

        public async Task<AttachFileReadDTO> CreateAttachFilesAsync(AttachFileWriteDTO dto)
        {

            var fileUpload = new UploadFileDTO
            {
                File = dto.AttachFile!,
                FileName = $"{dto.Name}-{Guid.NewGuid()}",
                FileType = FileTypeExtensions.ToFileMimeTypeString(dto.FileType)

            };

            var file = _mapper.Map<AttachFile>(dto);

            file.ReferenceLink = await _uploadFileService.UploadFileAsync(fileUpload);

            file.CreatedBy = _userContextService.Username! ??
                throw new CanNotAssignUserException();

            await _unitOfWork.AttachFileRepository.AddAsync(file);

            await _unitOfWork.CommitAsync();


            return _mapper.Map<AttachFileReadDTO>(file);
        }



        public Task<AttachFileReadDTO> GetReferenceAttachFile(string refId)
        {
            throw new NotImplementedException();
        }
    }
}
