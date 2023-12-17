using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using OfficeOpenXml;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;



namespace Metadata.Infrastructure.Services.Implementations
{
    public class LandTypeService : ILandTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LandTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LandTypeReadDTO?> CreateLandTypeAsync(LandTypeWriteDTO landTypeWriteDTO)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindAsync(landTypeWriteDTO.LandGroupId!)
                ?? throw new EntityWithIDNotFoundException<LandType>(landTypeWriteDTO.LandGroupId);

            await EnsureLandTypeCodeNotDuplicate(landTypeWriteDTO.Code, landTypeWriteDTO.Name);

            var landType = _mapper.Map<LandType>(landTypeWriteDTO);
            await _unitOfWork.LandTypeRepository.AddAsync(landType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandTypeReadDTO>(landType);
        }
        public async Task<LandTypeReadDTO?> CreateLandTypeForImportAsync(LandTypeWriteForImportDTO landTypeWriteDTO)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAsync(landTypeWriteDTO.LandGroupCode!)
                ?? throw new EntityWithIDNotFoundException<LandGroup>(landTypeWriteDTO.LandGroupCode);


            await EnsureLandTypeCodeNotDuplicate(landTypeWriteDTO.Code, landTypeWriteDTO.Name);

            var landType = _mapper.Map<LandType>(landTypeWriteDTO);
            landType.LandGroupId = landGroup.LandGroupId;
            await _unitOfWork.LandTypeRepository.AddAsync(landType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandTypeReadDTO>(landType);
        }

        public async Task<IEnumerable<LandTypeReadDTO>> CreateLandTypesAsync(IEnumerable<LandTypeWriteDTO> landTypeWriteDTOs)
        {
            var landTypes = new List<LandTypeReadDTO>();

            foreach (var landTypeDTO in landTypeWriteDTOs)
            {
                var landGroup = await _unitOfWork.LandGroupRepository.FindAsync(landTypeDTO.LandGroupId!)
                    ?? throw new EntityWithIDNotFoundException<LandGroup>(landTypeDTO.LandGroupId);
                await EnsureLandTypeCodeNotDuplicate(landTypeDTO.Code, landTypeDTO.Name);

                var landType = _mapper.Map<LandType>(landTypeDTO);

                await _unitOfWork.LandTypeRepository.AddAsync(landType);
                await _unitOfWork.CommitAsync();

                var readDTO = _mapper.Map<LandTypeReadDTO>(landType);

                landTypes.Add(readDTO);
            }

            return landTypes;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindAsync(id);
            if (landType == null)
            {
                throw new EntityWithIDNotFoundException<LandType>(id);
            }
            landType.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<IEnumerable<LandTypeReadDTO>> GetAllLandTypeAsync()
        {
            var landTypes = await _unitOfWork.LandTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LandTypeReadDTO>>(landTypes);
        }

        public async Task<IEnumerable<LandTypeReadDTO>> GetAllActivedLandTypeAsync()
        {
            var landTypes = await _unitOfWork.LandTypeRepository.GetAllActivedLandTypes();
            return _mapper.Map<IEnumerable<LandTypeReadDTO>>(landTypes);
        }

        public async Task<LandTypeReadDTO?> GetAsync(string code)
        {
            var landTypes = await _unitOfWork.LandTypeRepository.FindAsync(code);
            return _mapper.Map<LandTypeReadDTO>(landTypes);
        }

        public async Task<LandTypeReadDTO?> UpdateAsync(string id, LandTypeWriteDTO landTypeUpdateDTO)
        {
            var existLandType = await _unitOfWork.LandTypeRepository.FindAsync(id);
            if (existLandType == null)
            {
                throw new EntityWithIDNotFoundException<LandType>(id);
            }
            var landGroup = await _unitOfWork.LandGroupRepository.FindAsync(landTypeUpdateDTO.LandGroupId!)
                ?? throw new EntityWithIDNotFoundException<LandGroup>(landTypeUpdateDTO.LandGroupId);
            _mapper.Map(landTypeUpdateDTO, existLandType);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandTypeReadDTO>(existLandType);
        }

        private async Task EnsureLandTypeCodeNotDuplicate(string code, string name)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (landType != null && landType.Code == code)
            {
                throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType.Code), code);
            }
            var landType2 = await _unitOfWork.LandTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (landType2 != null && landType2.Name == name)
            {
                throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType2.Name), name);
            }
        }

        /* //EnsureLandTypeCodeNotDuplicateForUpdate
         public async Task EnsureLandTypeCodeNotDuplicateForUpdate(string id, string code, string name)
         {
             var landType = await _unitOfWork.LandTypeRepository.FindByCodeAndIsDeletedStatusForUpdate(code,id, false);
             if (landType != null && landType.Code == code && landType.LandTypeId != id)
             {
                 throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType.Code), code);
             }
             var landType2 = await _unitOfWork.LandTypeRepository.FindByNameAndIsDeletedStatusForUpdate(name,id, false);
             if (landType2 != null && landType2.Name == name && landType2.LandTypeId != id)
             {
                 throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType2.Name), name);
             }
         }*/

        public async Task CheckNameLandGroupNotDuplicate(string name)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindByNameAndIsDeletedStatus(name, false);
            if (landType != null && landType.Name == name)
            {
                throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType.Name), name);
            }
        }

        public async Task CheckCodeLandGroupNotDuplicate(string code)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (landType != null && landType.Code == code)
            {
                throw new UniqueConstraintException<LandTypeReadDTO>(nameof(landType.Code), code);
            }

        }



        public async Task<PaginatedResponse<LandTypeReadDTO>> QueryLandTypeAsync(LandTypeQuery paginationQuery)
        {
            var landTypes = await _unitOfWork.LandTypeRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<LandTypeReadDTO>.FromEnumerableWithMapping(landTypes, paginationQuery, _mapper);
        }

        public Task<IEnumerable<LandTypeReadDTO>> GetAllDeletedLandTypeAsync()
        {
            throw new NotImplementedException();
        }

        /*//import data from excel
        public async Task<List<LandTypeReadDTO>> ImportLandTypeFromExcelAsync(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found", filePath);

            List<LandTypeWriteDTO> landTypes = new List<LandTypeWriteDTO>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.End.Row;

                for (int row = 4; row <= totalRows; row++)
                {
                    string code = worksheet.Cells[row, 1].Text;
                    string name = worksheet.Cells[row, 2].Text;
                    string landGroupId = worksheet.Cells[row, 3].Text;
                    landTypes.Add(new LandTypeWriteDTO { Code = code, Name = name , LandGroupId = landGroupId });
                }
            }

            List<LandTypeReadDTO> importedObjects = new List<LandTypeReadDTO>();
            foreach (var sp in landTypes)
            {
                var importedObject = await CreateLandTypeAsync(sp);
                if (importedObject != null)
                {
                    importedObjects.Add(importedObject);
                }
            }
            return importedObjects;
        }*/
        //import data from excel
        public async Task<List<LandTypeReadDTO>> ImportLandTypeFromExcelAsync(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found", filePath);

            List<LandTypeWriteForImportDTO> landTypes = new List<LandTypeWriteForImportDTO>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.End.Row;

                for (int row = 4; row <= totalRows; row++)
                {
                    string code = worksheet.Cells[row, 1].Text;
                    string name = worksheet.Cells[row, 2].Text;
                    string landGroupCode = worksheet.Cells[row, 3].Text;

                    if (string.IsNullOrEmpty(code) ||
                        string.IsNullOrEmpty(name) ||
                        string.IsNullOrEmpty(landGroupCode))
                    {

                        continue;
                    }

                    landTypes.Add(new LandTypeWriteForImportDTO { Code = code, Name = name, LandGroupCode = landGroupCode });
                }
            }

            List<LandTypeReadDTO> importedObjects = new List<LandTypeReadDTO>();
            foreach (var sp in landTypes)
            {
                var importedObject = await CreateLandTypeForImportAsync(sp);
                if (importedObject != null)
                {
                    importedObjects.Add(importedObject);
                }
            }
            return importedObjects;
        }
    }
}
