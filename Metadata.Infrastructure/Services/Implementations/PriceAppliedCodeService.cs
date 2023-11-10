using Amazon.S3.Model;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class PriceAppliedCodeService : IPriceAppliedCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PriceAppliedCodeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PriceAppliedCodeReadDTO>> CreatePriceAppliedCodesAsync(IEnumerable<PriceAppliedCodeWriteDTO> dto)
        {
            var priceAppliedCodes = new List<PriceAppliedCodeReadDTO>();

            foreach (var item in dto)
            {
                var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(item.UnitPriceCode);

                if (existPriceAppliedCode != null)
                {
                    throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), item.UnitPriceCode);
                }

                var priceAppliedCode = _mapper.Map<PriceAppliedCode>(item);

               
                //await _unitOfWork.PriceAppliedCodeRepository.AddAsync(priceAppliedCode);
                
                var priceAppliedCodeReadDTO = _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);
                priceAppliedCodes.Add(priceAppliedCodeReadDTO);
            }
            await _unitOfWork.CommitAsync();
            return priceAppliedCodes;
        }

        public async Task DeletePriceAppliedCodeAsync(string Id)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(Id, include:"UnitPriceAssets");

            if (priceAppliedCode == null)
            {
                throw new EntityWithIDNotFoundException<PriceAppliedCode>(Id);
            }

            //Cannot Delete Price Applied Code that existed in UnitPrice Asset 
            if(priceAppliedCode.UnitPriceAssets.Count() > 0)
            {
                throw new InvalidActionException();
            }

            priceAppliedCode.IsDeleted = true;

            _unitOfWork.PriceAppliedCodeRepository.Delete(priceAppliedCode);

            await _unitOfWork.CommitAsync();
        }

        public async Task<PriceAppliedCodeReadDTO> GetPriceAppliedCodeAsync(string Id)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(Id, include: "UnitPriceAssets");

            return _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);
        }

        public async Task<PaginatedResponse<PriceAppliedCodeReadDTO>> QueryPriceAppliedCodeAsync(PriceAppliedCodeQuery paginationQuery)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.QueryAsync(paginationQuery);

            return PaginatedResponse<PriceAppliedCodeReadDTO>.FromEnumerableWithMapping(priceAppliedCode, paginationQuery, _mapper);
        }

        public async Task<PriceAppliedCodeReadDTO> UpdatePriceAppliedCodeAsync(string Id, PriceAppliedCodeWriteDTO dto)
        {
            var priceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.FindAsync(Id)
                ??throw new EntityWithIDNotFoundException<PriceAppliedCode>(Id);

            var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(dto.UnitPriceCode);

            if (existPriceAppliedCode != null) 
                throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), dto.UnitPriceCode);

            _mapper.Map(dto, priceAppliedCode);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PriceAppliedCodeReadDTO>(priceAppliedCode);
        }

        public async Task<PriceAppliedCodeReadDTO> CheckDuplicateCodeAsync(string code)
        {
            var existPriceAppliedCode = await _unitOfWork.PriceAppliedCodeRepository.GetPriceAppliedCodeByCodeAsync(code);
            if (existPriceAppliedCode != null)
                throw new UniqueConstraintException<PriceAppliedCode>(nameof(PriceAppliedCode.UnitPriceCode), code);
            return _mapper.Map<PriceAppliedCodeReadDTO>(existPriceAppliedCode);
        }

        
    }
}
