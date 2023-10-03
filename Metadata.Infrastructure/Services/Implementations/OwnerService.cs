using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public OwnerService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<OwnerReadDTO> CreateOwnerAsync(OwnerWriteDTO dto)
        {
            var owner = _mapper.Map<Owner>(dto);

            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.OwnerRepository.AddAsync(owner);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }

        /// <summary>
        /// This method only used to test export all owners in database
        /// </summary>
        /// <returns></returns>
        public async Task<ExportFileDTO> ExportOwnerFileAsync(string projectId)
        {
            var owners = await _unitOfWork.OwnerRepository.GetOwnersOfProjectAsync(projectId);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Owners");

                int row = 1;

                var properties = typeof(Owner).GetProperties();

                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = properties[col].Name;
                }

                foreach (var item in owners)
                {
                    row++;
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = properties[col].GetValue(item);
                    }
                }
                return new ExportFileDTO
                {
                    FileByte = package.GetAsByteArray(),
                    FileName = $"{"yolo" + $"{Guid.NewGuid()}"}"
                };
            }
        }

        public async Task DeleteOwner(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            _unitOfWork.OwnerRepository.Delete(owner);

            await _unitOfWork.CommitAsync();
        }

        public async Task<OwnerReadDTO> GetOwnerAsync(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);
            return _mapper.Map<OwnerReadDTO>(owner);
        }

        public Task ImportOwner(IFormFile attachFile)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResponse<OwnerReadDTO>> QueryOwnerAsync(OwnerQuery query)
        {
           var owner = await _unitOfWork.OwnerRepository.QueryAsync(query);
           return PaginatedResponse<OwnerReadDTO>.FromEnumerableWithMapping(owner, query, _mapper);
        }

        public async Task<OwnerReadDTO> UpdateOwnerAsync(string ownerId, OwnerWriteDTO dto)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            _mapper.Map(dto, owner);

            owner.OwnerCreatedBy = _userContextService.Username!
                ?? throw new CanNotAssignUserException();

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OwnerReadDTO>(owner);
        }
    }
}
