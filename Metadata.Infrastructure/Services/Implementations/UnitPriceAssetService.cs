using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Metadata.Core.Entities;
using Metadata.Core.Enums;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class UnitPriceAssetService : IUnitPriceAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitPriceAssetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UnitPriceAssetReadDTO> CreateUnitPriceAssetAsync(UnitPriceAssetWriteDTO dto)
        {
            var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(dto.AssetUnitId) 
                ?? throw new EntityWithIDNotFoundException<AssetUnit>(dto.AssetUnitId);

            var assetGroup = await _unitOfWork.AssetGroupRepository.FindAsync(dto.AssetGroupId)
                ?? throw new EntityWithIDNotFoundException<AssetGroup>(dto.AssetGroupId);

            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(dto.PriceAppliedCodeId)
                   ?? throw new EntityWithIDNotFoundException<PriceAppliedCode>(dto.PriceAppliedCodeId);

            var unitPriceAsset = _mapper.Map<UnitPriceAsset>(dto);

            await _unitOfWork.UnitPriceAssetRepository.AddAsync(unitPriceAsset);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<UnitPriceAssetReadDTO>(unitPriceAsset);
        }

        public async Task<IEnumerable<UnitPriceAssetReadDTO>> CreateUnitPriceAssetsAsync(IEnumerable<UnitPriceAssetWriteDTO> dtos)
        {
            var list = new List<UnitPriceAsset>();

            foreach(var dto in dtos)
            {
                var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(dto.AssetUnitId)
                    ?? throw new EntityWithIDNotFoundException<AssetUnit>(dto.AssetUnitId);

                var assetGroup = await _unitOfWork.AssetGroupRepository.FindAsync(dto.AssetGroupId)
                    ?? throw new EntityWithIDNotFoundException<AssetGroup>(dto.AssetGroupId);

                var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(dto.PriceAppliedCodeId)
                    ?? throw new EntityWithIDNotFoundException<PriceAppliedCode>(dto.PriceAppliedCodeId);

                var unitPriceAsset = _mapper.Map<UnitPriceAsset>(dto);

                await _unitOfWork.UnitPriceAssetRepository.AddAsync(unitPriceAsset);

                await _unitOfWork.CommitAsync();

                list.Add(unitPriceAsset);
            }

            return _mapper.Map<IEnumerable<UnitPriceAssetReadDTO>>(list);
        }

        public async Task DeleteUnitPriceAsset(string unitPriceAssetId)
        {
            var unitPriceAsset = await _unitOfWork.UnitPriceAssetRepository.FindAsync(unitPriceAssetId, include: "AssetCompensations");

            if (unitPriceAsset == null) throw new EntityWithIDNotFoundException<UnitPriceAssetReadDTO>(unitPriceAssetId);

            if(unitPriceAsset.AssetCompensations.Count() > 0)
            {
                throw new InvalidActionException();
            }

            unitPriceAsset.IsDeleted = true;

            _unitOfWork.UnitPriceAssetRepository.Delete(unitPriceAsset);

            await _unitOfWork.CommitAsync();
        }

        public async Task<UnitPriceAssetReadDTO> GetUnitPriceAssetAsync(string unitPriceAssetId)
        {
            return _mapper.Map<UnitPriceAssetReadDTO>(await _unitOfWork.UnitPriceAssetRepository.FindAsync(unitPriceAssetId));
        }

        public async Task<PaginatedResponse<UnitPriceAssetReadDTO>> UnitPriceAssetQueryAsync(UnitPriceAssetQuery query)
        {
            var unitPriceAsset = await _unitOfWork.UnitPriceAssetRepository.QueryAsync(query);

            return PaginatedResponse<UnitPriceAssetReadDTO>.FromEnumerableWithMapping(unitPriceAsset, query, _mapper);
        }

        public async Task<UnitPriceAssetReadDTO> UpdateUnitPriceAssetAsync(string unitPriceAssetId, UnitPriceAssetWriteDTO dto)
        {
            var unitPriceAsset = await _unitOfWork.UnitPriceAssetRepository.FindAsync(unitPriceAssetId);

            if (unitPriceAsset == null) throw new EntityWithIDNotFoundException<UnitPriceAsset>(unitPriceAssetId);

            var assetUnit = await _unitOfWork.AssetUnitRepository.FindAsync(dto.AssetUnitId)
                   ?? throw new EntityWithIDNotFoundException<AssetUnit>(dto.AssetUnitId);

            var assetGroup = await _unitOfWork.AssetGroupRepository.FindAsync(dto.AssetGroupId)
                ?? throw new EntityWithIDNotFoundException<AssetGroup>(dto.AssetGroupId);

            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(dto.PriceAppliedCodeId)
                ?? throw new EntityWithIDNotFoundException<PriceAppliedCode>(dto.PriceAppliedCodeId);

            _mapper.Map(dto, unitPriceAsset);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<UnitPriceAssetReadDTO>(unitPriceAsset);

        }

        public async Task<IEnumerable<UnitPriceAssetReadDTO>> GetUnitPriceAssetsOfProjectAsync(string projectId)
        {
            return _mapper.Map<IEnumerable<UnitPriceAssetReadDTO>>(await _unitOfWork.UnitPriceAssetRepository.GetUnitPriceAssetsOfProjectAsync(projectId));
        }

        public async Task<IEnumerable<UnitPriceAssetReadDTO>> ImportUnitPriceAssetFromExcelFileAsync(IFormFile file)
        {
            var dtos = await ExtractUnitPriceAssetsFromFileAsync(file);

            var unitPriceAssetList = new List<UnitPriceAsset>();

            foreach (var dto in dtos)
            {

                var unitPriceAsset = _mapper.Map<UnitPriceAsset>(dto);

                await _unitOfWork.UnitPriceAssetRepository.AddAsync(unitPriceAsset);

                unitPriceAssetList.Add(unitPriceAsset);
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<UnitPriceAssetReadDTO>>(unitPriceAssetList);
        }

        public async Task<IEnumerable<UnitPriceAssetWriteDTO>> ExtractUnitPriceAssetsFromFileAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length <= 0)
            {
                throw new InvalidActionException();
            }

            var importedUnitPriceAssets = new List<UnitPriceAssetFileImportWriteDTO>();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 11; row <= worksheet.Dimension.End.Row; row++)
                {
                    var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(worksheet.Cells[row, 8].Value?.ToString() ?? string.Empty)
                        ?? throw new EntityInputExcelException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), worksheet.Cells[row, 8].Value.ToString()!, row);
                    
                    var assetUnit = await _unitOfWork.AssetUnitRepository.FindByCodeAndIsDeletedStatus(worksheet.Cells[row, 9].Value?.ToString() ?? string.Empty, false)
                        ?? throw new EntityInputExcelException<AssetUnit>(nameof(AssetUnit.Code), worksheet.Cells[row, 9].Value.ToString()!, row);

                    var assetGroup = await _unitOfWork.AssetGroupRepository.FindByCodeAndIsDeletedStatus(worksheet.Cells[row, 10].Value?.ToString() ?? string.Empty, false)
                       ?? throw new EntityInputExcelException<AssetGroup>(nameof(AssetGroup.Code), worksheet.Cells[row, 10].Value.ToString()!, row);

                    var unitPriceAsset = new UnitPriceAssetFileImportWriteDTO
                    {

                        AssetName = worksheet.Cells[row, 4].Value?.ToString()!,
                        AssetPrice = decimal.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? "0"),
                        AssetRegulation = worksheet.Cells[row, 6].Value?.ToString() ?? string.Empty,
                        AssetType = MapAssetTypeEnumWithUserInput(worksheet.Cells[row, 7].Value?.ToString()!).ToString()
                            ?? throw new EntityInputExcelException<UnitPriceAsset>(nameof(UnitPriceAsset.AssetType), worksheet.Cells[row, 7].Value.ToString()!, row),
                        PriceAppliedCodeId = priceAppliedCode.PriceAppliedCodeId,
                        AssetUnitId = assetUnit.AssetUnitId,
                        AssetGroupId = assetGroup.AssetGroupId

                    };

                    importedUnitPriceAssets.Add(unitPriceAsset);
                }
                package.Dispose();
            }
            return _mapper.Map<IEnumerable<UnitPriceAssetWriteDTO>>(importedUnitPriceAssets);
        }

        private static AssetOnLandTypeEnum MapAssetTypeEnumWithUserInput(string typeName)
        {
            var input = typeName.ToLower().Split(" ");
            if (input.Equals("nhà")) return AssetOnLandTypeEnum.House;
            if (input.Equals("kiếntrúc")) return AssetOnLandTypeEnum.Architecture;
            if (input.Equals("câytrồng")) return AssetOnLandTypeEnum.Plants;
            return AssetOnLandTypeEnum.Other;
        }
    }
}
