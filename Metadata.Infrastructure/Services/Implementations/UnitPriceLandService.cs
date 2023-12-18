using Amazon.S3.Model;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class UnitPriceLandService : IUnitPriceLandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitPriceLandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UnitPriceLandReadDTO> CreateUnitPriceLandAsync(UnitPriceLandWriteDTO dto)
        {
            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId)
                ?? throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var landType = await _unitOfWork.AssetGroupRepository.FindAsync(dto.LandTypeId)
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            var unitPriceLand = _mapper.Map<UnitPriceLand>(dto);

            await _unitOfWork.UnitPriceLandRepository.AddAsync(unitPriceLand);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<UnitPriceLandReadDTO>(unitPriceLand);
        }


        public async Task<IEnumerable<UnitPriceLandReadDTO>> CreateUnitPriceLandsAsync(IEnumerable<UnitPriceLandWriteDTO> dtos)
        {

            var list = new List<UnitPriceLand>();

            foreach (var item in dtos)
            {
                var project = await _unitOfWork.ProjectRepository.FindAsync(item.ProjectId)
                ?? throw new EntityWithIDNotFoundException<Project>(item.ProjectId);

                var landType = await _unitOfWork.LandTypeRepository.FindAsync(item.LandTypeId)
                    ?? throw new EntityWithIDNotFoundException<LandType>(item.LandTypeId);

                var unitPriceLand = _mapper.Map<UnitPriceLand>(item);

                await _unitOfWork.UnitPriceLandRepository.AddAsync(unitPriceLand);

                await _unitOfWork.CommitAsync();

                list.Add(unitPriceLand);
            }

            return _mapper.Map<IEnumerable<UnitPriceLandReadDTO>>(list);
        }


        public async Task DeleteUnitPriceLand(string unitPriceLandId)
        {
            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(unitPriceLandId);


            if (unitPriceLand == null) throw new EntityWithIDNotFoundException<UnitPriceLand>(unitPriceLand);

            var project = await _unitOfWork.ProjectRepository.FindAsync(unitPriceLand.ProjectId, include: "Owners")
              ?? throw new EntityWithIDNotFoundException<Project>(unitPriceLand.ProjectId);

            if (!project.Owners.IsNullOrEmpty())
            {
                throw new InvalidActionException("Không Xóa Được Đơn Giá Đất Do Dự Án Đã Có Chủ Sở Hữu");
            }

            unitPriceLand.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task<UnitPriceLandReadDTO> GetUnitPriceLandAsync(string unitPriceLandId)
        {
            return _mapper.Map<UnitPriceLandReadDTO>(await _unitOfWork.UnitPriceLandRepository.FindAsync(unitPriceLandId));
        }

        public async Task<PaginatedResponse<UnitPriceLandReadDTO>> UnitPriceLandQueryAsync(UnitPriceLandQuery query)
        {
            var unitPriceLands = await _unitOfWork.UnitPriceLandRepository.QueryAsync(query);

            return PaginatedResponse<UnitPriceLandReadDTO>.FromEnumerableWithMapping(unitPriceLands, query, _mapper);
        }

        public async Task<UnitPriceLandReadDTO> UpdateUnitPriceLandAsync(string unitPriceLandId, UnitPriceLandWriteDTO dto)
        {
            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.FindAsync(unitPriceLandId);

            if (unitPriceLand == null) throw new EntityWithIDNotFoundException<UnitPriceLand>(unitPriceLandId);

            var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId, include: "Owners")
                ?? throw new EntityWithIDNotFoundException<Project>(dto.ProjectId);

            var landType = await _unitOfWork.LandTypeRepository.FindAsync(dto.LandTypeId)
                ?? throw new EntityWithIDNotFoundException<LandType>(dto.LandTypeId);

            if (project.Owners.Count() > 0)
            {
                throw new InvalidActionException("Không Cập Nhật Được Đơn Giá Đất Do Dự Án Đã Có Chủ Sở Hữu");
            }

            _mapper.Map(dto, unitPriceLand);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<UnitPriceLandReadDTO>(unitPriceLand);
        }

        public async Task<PaginatedResponse<UnitPriceLandReadDTO>> QueryUnitPriceLandOfProjectAsync(string projectId, UnitPriceLandQuery query)
        {
            var unitPriceLand = await _unitOfWork.UnitPriceLandRepository.QueryUnitPriceLandOfProjectAsync(projectId, query);
            return PaginatedResponse<UnitPriceLandReadDTO>.FromEnumerableWithMapping(unitPriceLand, query, _mapper);
        }

        public async Task<IEnumerable<UnitPriceLandReadDTO>> ImportUnitPriceLandFromExcelFileAsync(IFormFile file)
        {
            var dtos = await ExtractUnitPriceLandsFromFileAsync(file);

            var unitPriceLandList = new List<UnitPriceLand>();

            foreach (var dto in dtos)
            {
                var project = await _unitOfWork.ProjectRepository.FindAsync(dto.ProjectId!);

                if (project == null) throw new EntityWithIDNotFoundException<Project>(dto.ProjectId!);

                var unitPriceLand = _mapper.Map<UnitPriceLand>(dto);

                await _unitOfWork.UnitPriceLandRepository.AddAsync(unitPriceLand);

                unitPriceLandList.Add(unitPriceLand);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<UnitPriceLandReadDTO>>(unitPriceLandList);
        }

        public async Task<IEnumerable<UnitPriceLandWriteDTO>> ExtractUnitPriceLandsFromFileAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length <= 0)
            {
                throw new InvalidActionException();
            }

            var importedUnitPriceLands = new List<UnitPriceLandFileImportWriteDTO>();
            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                string[] parts = worksheet.Cells["D6"].Text.Split(':');

                var project = await _unitOfWork.ProjectRepository.GetProjectByProjectCodeAsync(parts[1].Trim());

                if (project == null)
                    throw new EntityWithAttributeNotFoundException<Project>(nameof(Project.ProjectName), parts[1].Trim());
                
                for (int row = 11; row <= worksheet.Dimension.End.Row; row++)
                {

                    var landType = await _unitOfWork.LandTypeRepository.FindByCodeAndIsDeletedStatus(worksheet.Cells[row, 5].Value?.ToString() ?? string.Empty, false)
                        ?? throw new EntityInputExcelException<LandType>(nameof(UnitPriceLand.LandType), worksheet.Cells[row, 5].Value.ToString()!, row);

                    var unitPriceLand = new UnitPriceLandFileImportWriteDTO
                    {
                        ProjectId = project.ProjectId,
                        StreetAreaName = worksheet.Cells[row, 4].Value?.ToString()!,
                        LandTypeId = landType.LandTypeId!,
                        LandUnit = worksheet.Cells[row, 6].Value?.ToString()!,
                        LandPosition1 = decimal.Parse(worksheet.Cells[row, 7].Value?.ToString() ?? "0"),
                        LandPosition2 = decimal.Parse(worksheet.Cells[row, 8].Value?.ToString() ?? "0"),
                        LandPosition3 = decimal.Parse(worksheet.Cells[row, 9].Value?.ToString() ?? "0"),
                        LandPosition4 = decimal.Parse(worksheet.Cells[row, 10].Value?.ToString() ?? "0"),
                        LandPosition5 = decimal.Parse(worksheet.Cells[row, 11].Value?.ToString() ?? "0"),

                    };

                    importedUnitPriceLands.Add(unitPriceLand);

                }
                package.Dispose();
            }
            return _mapper.Map<IEnumerable<UnitPriceLandWriteDTO>>(importedUnitPriceLands);
        }
    }
}
