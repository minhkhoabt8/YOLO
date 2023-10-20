using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.Services.Interfaces;


namespace Metadata.Infrastructure.Services.Implementations
{
    public class DeductionService : IDeductionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public DeductionService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<DeductionReadDTO>> CreateOwnerDeductionsAsync(string ownerId, IEnumerable<DeductionWriteDTO> dto)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId);

            if (owner == null) throw new EntityWithIDNotFoundException<Owner>(ownerId);

            if (dto == null) throw new InvalidActionException(nameof(dto));

            var deductionList = new List<Deduction>();

            foreach (var item in dto)
            {
                var deduction = _mapper.Map<Deduction>(item);

                deduction.OwnerId = ownerId;

                await _unitOfWork.DeductionRepository.AddAsync(deduction);

                deductionList.Add(deduction);
            }
            await _unitOfWork.CommitAsync();

            return _mapper.Map<IEnumerable<DeductionReadDTO>>(deductionList);
        }

        public async Task DeleteDeductionAsync(string deductionId)
        {
            var deduction = await _unitOfWork.DeductionRepository.FindAsync(deductionId);

            if (deduction == null) throw new EntityWithIDNotFoundException<Deduction>(deductionId);

            _unitOfWork.DeductionRepository.Delete(deduction);

            await _unitOfWork.CommitAsync();
        }

        public async Task<DeductionReadDTO> UpdateDeductionAsync(string deductionId, DeductionWriteDTO dto)
        {
            var deduction = await _unitOfWork.DeductionRepository.FindAsync(deductionId);

            if (deduction == null) throw new EntityWithIDNotFoundException<Deduction>(deductionId);

            _mapper.Map(dto, deduction);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<DeductionReadDTO>(deduction);
        }
    }
}
