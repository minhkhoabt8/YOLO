using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.DTOs.SupportType;
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
    public class DeductionTypeService : IDeductionTypeService
    {   
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeductionTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DeductionTypeReadDTO> AddDeductionType(DeductionTypeWriteDTO deductionType)
        {
            await EnsureDeductionTypeNotExist(deductionType.Code,deductionType.Name);
            var newDeductionType = _mapper.Map<DeductionType>(deductionType);
            await _unitOfWork.DeductionTypeRepository.AddAsync(newDeductionType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DeductionTypeReadDTO>(newDeductionType);
        }



        public async Task<bool> DeleteDeductionTypeAsync(string id)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindAsync(id);
            if (existDeductionType == null)
            {
                throw new EntityWithIDNotFoundException<DeductionType>(id);
            }
            existDeductionType.IsDeleted = true;
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<DeductionTypeReadDTO>> GetAllDeductionTypesAsync()
        {
            var deductionTypes = await _unitOfWork.DeductionTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DeductionTypeReadDTO>>(deductionTypes);
        }

        public async Task<IEnumerable<DeductionTypeReadDTO>> GetActivedDeductionTypes()
        {
            var deductionTypes = await _unitOfWork.DeductionTypeRepository.GetActivedDeductionTypes();
            return _mapper.Map<IEnumerable<DeductionTypeReadDTO>>(deductionTypes);
        }

        public async Task<DeductionTypeReadDTO> GetDeductionTypeAsync(string id)
        {
            var deductionType = await _unitOfWork.DeductionTypeRepository.FindAsync(id);
            return _mapper.Map<DeductionTypeReadDTO>(deductionType);
        }

        public async Task<DeductionTypeReadDTO> UpdateDeductionTypeAsync(string id, DeductionTypeWriteDTO deductionType)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindAsync(id);
            if (existDeductionType == null)
            {
                throw new EntityWithIDNotFoundException<DeductionType>(id);
            }
            await EnsureDeductionTypeNotExistForUpdate(deductionType.Code, deductionType.Name, id);
            _mapper.Map(deductionType, existDeductionType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DeductionTypeReadDTO>(existDeductionType);
        }

        private async Task EnsureDeductionTypeNotExist(string code, string name)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (existDeductionType != null && existDeductionType.Code==code)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType.Code),code);
            }
            var existDeductionType2 = await _unitOfWork.DeductionTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (existDeductionType2 != null && existDeductionType2.Name == name)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType2.Name), name);
            }
        }

        //EnsureDeductionTypeNotExistForUpdate
        private async Task EnsureDeductionTypeNotExistForUpdate(string code, string name, string id)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindByCodeAndIsDeletedStatusForUpdate(code,id, false);
            if (existDeductionType != null && existDeductionType.Code == code && existDeductionType.DeductionTypeId != id)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType.Code), code);
            }
            var existDeductionType2 = await _unitOfWork.DeductionTypeRepository.FindByCodeAndIsDeletedStatusForUpdate(name,id, false);
            if (existDeductionType2 != null && existDeductionType2.Name == name && existDeductionType2.DeductionTypeId != id)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType2.Name), name);
            }
        }

        public async Task ChecknameDeductionTypeNotDuplicate (string name)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (existDeductionType != null && existDeductionType.Name == name)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType.Name), name);
            }
        }

        public async Task CheckcodeDeductionTypeNotDuplicate(string code)
        {
            var existDeductionType = await _unitOfWork.DeductionTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (existDeductionType != null && existDeductionType.Code == code)
            {
                throw new UniqueConstraintException<DeductionType>(nameof(existDeductionType.Code), code);
            }
        }

        public async Task<IEnumerable<DeductionTypeReadDTO>> CreateListDeductionTypes(IEnumerable<DeductionTypeWriteDTO> WriteDTOs)
        {
            var deductionTypes = new List<DeductionTypeReadDTO>();
            foreach (var item in WriteDTOs)
            {
                await EnsureDeductionTypeNotExist(item.Code, item.Name);
                var newDeductionType = _mapper.Map<DeductionType>(item);
                await _unitOfWork.DeductionTypeRepository.AddAsync(newDeductionType);
                await _unitOfWork.CommitAsync();

                var readDTO = _mapper.Map<DeductionTypeReadDTO>(newDeductionType);
                deductionTypes.Add(readDTO);
            }
            return deductionTypes;
        }

        //QueryDeductionTypes paginateresponse 
        public async Task<PaginatedResponse<DeductionTypeReadDTO>> QueryDeductionTypesAsync(DeductionTypeQuery paginationQuery)
        {
            var deductionTypes = await _unitOfWork.DeductionTypeRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<DeductionTypeReadDTO>.FromEnumerableWithMapping(deductionTypes, paginationQuery, _mapper);
        }


        //import data from excel
        public async Task<List<DeductionTypeReadDTO>> ImportDeductionTypeFromExcelAsync(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found", filePath);

            List<DeductionTypeWriteDTO> deductionTypes = new List<DeductionTypeWriteDTO>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.End.Row;

                for (int row = 4; row <= totalRows; row++)
                {
                    string code = worksheet.Cells[row, 1].Text;
                    string name = worksheet.Cells[row, 2].Text;

                    deductionTypes.Add(new DeductionTypeWriteDTO { Code = code, Name = name });
                }
            }

            List<DeductionTypeReadDTO> importedObjects = new List<DeductionTypeReadDTO>();
            foreach (var sp in deductionTypes)
            {
                var importedObject = await AddDeductionType(sp);
                if (importedObject != null)
                {
                    importedObjects.Add(importedObject);
                }
            }
            return importedObjects;
        }
    }
}
