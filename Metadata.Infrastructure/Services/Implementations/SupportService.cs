using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class SupportService : ISupportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public SupportService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<SupportReadDTO>> CreateOwnerSupportsAsync(string ownerId, IEnumerable<SupportWriteDTO> dto)
        {
            //var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            //if(owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId); 

            if(dto == null) throw new InvalidActionException(nameof(dto));

            var supportList = new List<Support>();

            foreach(var item in dto)
            {
                var support = _mapper.Map<Support>(item);

                support.OwnerId = ownerId;
                
                await _unitOfWork.SupportRepository.AddAsync(support);

                supportList.Add(support);
            }

            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<SupportReadDTO>>(supportList);
        }


        public async Task<IEnumerable<SupportReadDTO>> GetSupportsAsync(string ownerId)
        {
            var supports = await _unitOfWork.SupportRepository.GetAllSupportsOfOwnerAsync(ownerId);

            if(supports == null) throw new EntityWithIDNotFoundException<Support>(ownerId);

            return _mapper.Map<IEnumerable<SupportReadDTO>>(supports);
        }

        public async Task DeleteSupportAsync(string supportId)
        {
            var support = await _unitOfWork.SupportRepository.FindAsync(supportId);

            if(support == null) throw new EntityWithIDNotFoundException<Support>(supportId);

            _unitOfWork.SupportRepository.Delete(support);

            await _unitOfWork.CommitAsync();
        }

        public async Task<SupportReadDTO> UpdateSupportAsync(string supportId, SupportWriteDTO dto)
        {
            var support = await _unitOfWork.SupportRepository.FindAsync(supportId);

            if (support == null) throw new EntityWithIDNotFoundException<Support>(supportId);

            _mapper.Map(dto, support);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<SupportReadDTO>(support);
        }

        public async Task<PaginatedResponse<SupportReadDTO>> QuerySupportAsync(SupportQuery paginationQuery)
        {
            var asset = await _unitOfWork.SupportRepository.QueryAsync(paginationQuery);
            return PaginatedResponse<SupportReadDTO>.FromEnumerableWithMapping(asset, paginationQuery, _mapper);
        }
    }
}
