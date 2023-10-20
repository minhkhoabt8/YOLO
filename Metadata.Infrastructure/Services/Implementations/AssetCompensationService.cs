using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.Services.Interfaces;


namespace Metadata.Infrastructure.Services.Implementations
{
    public class AssetCompensationService : IAssetCompensationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAttachFileService _attachFileService;

        public AssetCompensationService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, IAttachFileService attachFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _attachFileService = attachFileService;
        }

        public async Task<IEnumerable<AssetCompensationReadDTO>> CreateOwnerAssetCompensationsAsync(string ownerId, IEnumerable<AssetCompensationWriteDTO> dto)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (dto == null) throw new InvalidActionException(nameof(dto));

            var compensationList = new List<AssetCompensation>();

            foreach (var item in dto)
            {
                var compensation = _mapper.Map<AssetCompensation>(item);

                compensation.OwnerId = ownerId;

                await _unitOfWork.AssetCompensationRepository.AddAsync(compensation);

                foreach (var file in item.AttachFiles!)
                {
                    file.AssetCompensationId = compensation.AssetCompensationId;
                }

                await _attachFileService.UploadAttachFileAsync(item.AttachFiles!);

                compensationList.Add(compensation);
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<AssetCompensationReadDTO>>(compensationList);
        }

        public async Task DeleteAssetCompensationAsync(string compensationId)
        {
            var compensation = await _unitOfWork.AssetCompensationRepository.FindAsync(compensationId);

            if (compensation == null) throw new EntityWithIDNotFoundException<AssetCompensation>(compensationId);

            _unitOfWork.AssetCompensationRepository.Delete(compensation);

            await _unitOfWork.CommitAsync();
        }

        public async Task<AssetCompensationReadDTO> UpdateAssetCompensationAsync(string compensationId, AssetCompensationWriteDTO dto)
        {
            var compensation = await _unitOfWork.AssetCompensationRepository.FindAsync(compensationId);

            if (compensation == null) throw new EntityWithIDNotFoundException<AssetCompensation>(compensationId);

            _mapper.Map(dto, compensation);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AssetCompensationReadDTO>(compensation);
        }
    }
}
