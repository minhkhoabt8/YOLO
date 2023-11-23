using Amazon.S3.Model;
using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.LandResettlement;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class LandResettlementService : ILandResettlementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LandResettlementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LandResettlementReadDTO> CreateLandResettlementAsync(LandResettlementWriteDTO dto)
        {
            if (!dto.ResettlementProjectId.IsNullOrEmpty())
            {
                var resettlementProject = await _unitOfWork.ResettlementProjectRepository.FindAsync(dto.ResettlementProjectId!)
                 ?? throw new EntityWithIDNotFoundException<ResettlementProject>(dto.ResettlementProjectId!);
            }
            if (!dto.OwnerId.IsNullOrEmpty())
            {
                var owner = await _unitOfWork.OwnerRepository.FindAsync(dto.OwnerId!)
                 ?? throw new EntityWithIDNotFoundException<Core.Entities.Owner>(dto.OwnerId!);
            }

            var resettlement = _mapper.Map<LandResettlement>(dto);

            await _unitOfWork.LandResettlementRepository.AddAsync(resettlement);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandResettlementReadDTO>(resettlement);

        }

        public Task<IEnumerable<LandResettlementReadDTO>> CreateLandResettlementsAsync(IEnumerable<LandResettlementWriteDTO> dto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLandResettlementAsync(string id)
        {
            var landResettlement = await _unitOfWork.LandResettlementRepository.FindAsync(id)
                ?? throw new EntityWithIDNotFoundException<LandResettlement>(id);

            _unitOfWork.LandResettlementRepository.Delete(landResettlement);

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<LandResettlementReadDTO>> GetAllLandResettlementsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<LandResettlementReadDTO> GetLandResettlementAsync(string id)
        {
            var landResettlement = await _unitOfWork.LandResettlementRepository.FindAsync(id, include: "ResettlementProject, Owner")
                ?? throw new EntityWithIDNotFoundException<LandResettlement>(id);
            return _mapper.Map<LandResettlementReadDTO>(landResettlement);
        }

        public async Task<IEnumerable<LandResettlementReadDTO>> GetLandResettlementsOfOwnerAsync(string ownerId)
        {
            var owner = await _unitOfWork.OwnerRepository.FindAsync(ownerId)
                ?? throw new EntityWithIDNotFoundException<Core.Entities.Owner>(ownerId);

            return _mapper.Map<IEnumerable<LandResettlementReadDTO>>(await _unitOfWork.LandResettlementRepository.GetLandResettlementsOfOwnerIncludeResettlementProjectAsync(ownerId));
        }

        public async Task<IEnumerable<LandResettlementReadDTO>> GetLandResettlementsOfResettlementProjectAsync(string resettlementProjectId)
        {
            var resettlementProject = await _unitOfWork.LandResettlementRepository.FindAsync(resettlementProjectId)
                ?? throw new EntityWithIDNotFoundException<LandResettlement>(resettlementProjectId);

            return _mapper.Map<IEnumerable<LandResettlementReadDTO>>(await _unitOfWork.LandResettlementRepository.GetLandResettlementsOfResettlementProjectIncludeOwnerAsync(resettlementProjectId));
        }

        public async Task<LandResettlementReadDTO> UpdateLandResettlementAsync(string id, LandResettlementWriteDTO dto)
        {
            var landResettlement = await _unitOfWork.LandResettlementRepository.FindAsync(id);

            if (landResettlement == null) throw new EntityWithIDNotFoundException<LandResettlement>(id);

            _mapper.Map(dto, landResettlement);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<LandResettlementReadDTO>(landResettlement);
        }
    }
}
