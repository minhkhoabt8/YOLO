using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
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
    public class LandGroupService : ILandGroupService
    {   
        private IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public LandGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(string delete)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindAsync(delete, include: "LandTypes");
            if (landGroup == null)
            {
                throw new EntityWithIDNotFoundException<LandGroup>(delete);
            }

            if (!landGroup.LandTypes.IsNullOrEmpty())
            {
                throw new InvalidActionException($"Không thể xóa Nhóm đất: [{landGroup.Code}].");
            }

            landGroup.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return true;

        }

        public async Task<IEnumerable<LandGroupReadDTO>> GetAllLandGroupAsync()
        {
           var landGroups = await _unitOfWork.LandGroupRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LandGroupReadDTO>>(landGroups);
        }

        public async Task<IEnumerable<LandGroupReadDTO>> GetAllActivedLandGroupAsync()
        {
            var landGroups = await _unitOfWork.LandGroupRepository.GetAllActivedLandGroups();
            return _mapper.Map<IEnumerable<LandGroupReadDTO>>(landGroups);
        }

        public async Task<LandGroupReadDTO?> GetAsync(string code)
        {
            var landGroups = await _unitOfWork.LandGroupRepository.FindAsync(code);
            return _mapper.Map<LandGroupReadDTO>(landGroups);
        }

        public async Task<LandGroupReadDTO?> UpdateAsync(string id, LandGroupWriteDTO landGroupUpdateDTO)
        {
            var existLandgroup = await _unitOfWork.LandGroupRepository.FindAsync(id);

            if (existLandgroup == null)
            {
                throw new EntityWithIDNotFoundException<LandGroup>(id);
            }
           await EnsureAssetGroupCodeNotDuplicateForUpdate(landGroupUpdateDTO.Code, landGroupUpdateDTO.Name,id);
            _mapper.Map(landGroupUpdateDTO, existLandgroup);
            
            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandGroupReadDTO>(existLandgroup);

        }

        public async Task<LandGroupReadDTO?> CreateLandgroupAsync(LandGroupWriteDTO landGroupWriteDTO)
        {
            await EnsureLandGroupCodeNotDuplicate(landGroupWriteDTO.Code , landGroupWriteDTO.Name);
            
            var landGroup = _mapper.Map<LandGroup>(landGroupWriteDTO);
            await _unitOfWork.LandGroupRepository.AddAsync(landGroup);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LandGroupReadDTO>(landGroup);
        }
        public async Task<IEnumerable<LandGroupReadDTO>> CreateListLandGroupAsync(IEnumerable<LandGroupWriteDTO> landGroupWriteDTOs)
        { 
            var landGroups = new List<LandGroupReadDTO>();
            foreach (var landGroupWriteDTO in landGroupWriteDTOs)
            {
                await EnsureLandGroupCodeNotDuplicate(landGroupWriteDTO.Code, landGroupWriteDTO.Name);
                var landGroup = _mapper.Map<LandGroup>(landGroupWriteDTO);
                await _unitOfWork.LandGroupRepository.AddAsync(landGroup);
                await _unitOfWork.CommitAsync();

                var landGroupReadDTO = _mapper.Map<LandGroupReadDTO>(landGroup);
                landGroups.Add(landGroupReadDTO);
            }
            return landGroups;
        }

        private async Task EnsureLandGroupCodeNotDuplicate(string code,string name)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAndIsDeletedStatus(code,false);
            if (landGroup != null && landGroup.Code == code )
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Code), code);
            }
            
            var landGroup2 = await _unitOfWork.LandGroupRepository.FindByNameAndIsDeletedStatus(name, false);
            if (landGroup2 != null  && landGroup2.Name == name)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup2.Name), name);
            }
        }

        private async Task EnsureAssetGroupCodeNotDuplicateForUpdate(string code, string name, string id)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAndIsDeletedStatusForUpdate(code, id, false);
            if (landGroup != null && landGroup.Code == code && landGroup.LandGroupId != id)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Code), code);
            }
            var landGroup2 = await _unitOfWork.LandGroupRepository.FindByNameAndIsDeletedStatusForUpdate(name, id, false);
            if (landGroup2 != null && landGroup2.Name == name && landGroup2.LandGroupId != id)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup2.Name), name);
            }
        }

        public async Task CheckNameLandGroupNotDuplicate(string name)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByNameAndIsDeletedStatus(name, false);
            if (landGroup != null && landGroup.Name == name)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Name), name);
            }
        }
        public async Task CheckCodeLandGroupNotDuplicate(string code)
        {
            var landGroup = await _unitOfWork.LandGroupRepository.FindByCodeAndIsDeletedStatus(code, false);
            if (landGroup != null && landGroup.Code == code)
            {
                throw new UniqueConstraintException<LandGroup>(nameof(landGroup.Code), code);
            }
            
        }
       
        
       

        public async Task<PaginatedResponse<LandGroupReadDTO>> QueryLandGroupAsync(LandGroupQuery paginationQuery)
        {
            var landGroups = await _unitOfWork.LandGroupRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<LandGroupReadDTO>.FromEnumerableWithMapping(landGroups, paginationQuery, _mapper);
        }



        //import data from excel
        public async Task<List<LandGroupReadDTO>> ImportLandGroupsFromExcelAsync(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found", filePath);

            List<LandGroupWriteDTO> landGroups = new List<LandGroupWriteDTO>();
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
                    landGroups.Add(new LandGroupWriteDTO { Code = code, Name = name });
                }
            }

            List<LandGroupReadDTO> importedObjects = new List<LandGroupReadDTO>();
            foreach (var sp in landGroups)
            {
                var importedObject = await CreateLandgroupAsync(sp);
                if (importedObject != null)
                {
                    importedObjects.Add(importedObject);
                }
            }
            return importedObjects;
        }
    }
}
