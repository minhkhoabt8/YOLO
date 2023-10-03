using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using System.Data;

namespace Metadata.Infrastructure.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Project
            CreateMap<Project, ProjectReadDTO>();
            CreateMap<ProjectWriteDTO, Project>();

            // Document
            CreateMap<Document, DocumentReadDTO>();
            CreateMap<DocumentWriteDTO, Document>();

            //PriceApplyCode
            CreateMap<PriceAppliedCode, PriceAppliedCodeReadDTO>();

            //Owner
            CreateMap<Owner, OwnerReadDTO>();
            CreateMap<OwnerWriteDTO, Owner>();
            CreateMap<Owner, OwnersInProjectDTO>();

            //Plan
            CreateMap<Plan, PlanReadDTO>();
            CreateMap<Plan, PlansInProjectDTO>();   

            //LandPositionInfos
            CreateMap<LandPositionInfo, LandPositionInfoReadDTO>();

            //UnitPriceLand
            CreateMap<UnitPriceLand, UnitPriceLandReadDTO>();

        }
    }
}
