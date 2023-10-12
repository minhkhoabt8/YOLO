using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.SupportType;
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
            CreateMap<DocumentInProjectWriteDTO,Document>();

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
            //Audit Trail
            CreateMap<AuditTrail,AuditTrailReadDTO>();
            CreateMap<AuditTrailWriteDTO, AuditTrail>();

            //LandGroup
            CreateMap<LandGroup, LandGroupReadDTO>();
            CreateMap<LandGroupWriteDTO, LandGroup>();

            //LandType
            CreateMap<LandType, LandTypeReadDTO>();
            CreateMap<LandTypeWriteDTO, LandType>();

            //SupportType
            CreateMap<SupportType, SupportTypeReadDTO>();
            CreateMap<SupportTypeWriteDTO, SupportType>();

            //AssetGroup
            CreateMap<AssetGroup, AssetGroupReadDTO>();
            CreateMap<AssetGroupWriteDTO, AssetGroup>();

            //OrganizationType
            CreateMap<OrganizationType, OrganizationTypeReadDTO>();
            CreateMap<OrganizationTypeWriteDTO, OrganizationType>();

            //DeductionType
            CreateMap<DeductionType, DeductionTypeReadDTO>();
            CreateMap<DeductionTypeWriteDTO, DeductionType>();

            //DocumentType
            CreateMap<DocumentType, DocumentTypeReadDTO>();
            CreateMap<DocumentTypeWriteDTO, DocumentType>();

            //AssetUnit
            CreateMap<AssetUnit, AssetUnitReadDTO>();
            CreateMap<AssetUnitWriteDTO, AssetUnit>();

        }
    }
}
