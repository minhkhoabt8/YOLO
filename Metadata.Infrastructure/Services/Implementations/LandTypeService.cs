using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
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
                ?? throw new EntityWithIDNotFoundException<LandGroup>(landTypeWriteDTO.LandGroupId);

            await EnsureLandGroupCodeNotDuplicate(landTypeWriteDTO.Code , landTypeWriteDTO.Name);
            
            var landType = _mapper.Map<LandType>(landTypeWriteDTO);
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
                await EnsureLandGroupCodeNotDuplicate(landTypeDTO.Code, landTypeDTO.Name);

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

        private async Task EnsureLandGroupCodeNotDuplicate(string code,  string name)
        {
            var landType = await _unitOfWork.LandTypeRepository.FindByCodeAndIsDeletedStatus(code,false);
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
        
        public async Task CheckNameLandGroupNotDuplicate (string name)
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
    }
}
