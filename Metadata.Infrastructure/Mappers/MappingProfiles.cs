using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetGroup;
using Metadata.Infrastructure.DTOs.AssetUnit;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.AuditTrail;
using Metadata.Infrastructure.DTOs.DeductionType;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.DocumentType;
using Metadata.Infrastructure.DTOs.LandGroup;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.DTOs.OrganizationType;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.SupportType;
using Metadata.Infrastructure.DTOs.Support;
using Metadata.Infrastructure.DTOs.UnitPriceLand;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.DTOs.LandResettlement;

namespace Metadata.Infrastructure.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Project
            CreateMap<Project, ProjectReadDTO>()
                .ForMember(des => des.ProjectDocuments, act => act.MapFrom(src => src.ProjectDocuments.Select(pd => pd.Document)));

            CreateMap<ProjectWriteDTO, Project>();

            CreateMap<Project, ProjectInResettlementReadDTO>();

            // Document
            CreateMap<Document, DocumentReadDTO>();
            CreateMap<DocumentWriteDTO, Document>();
            CreateMap<DocumentInProjectWriteDTO,Document>();

            //PriceApplyCode
            CreateMap<PriceAppliedCode, PriceAppliedCodeReadDTO>()
                .ForMember(des => des.Documents, act => act.MapFrom(src => src.PriceAppliedCodeDocuments.Select(pd => pd.Document)));
            CreateMap<PriceAppliedCodeWriteDTO, PriceAppliedCode>();

            //Owner
            CreateMap<Owner, OwnerReadDTO>();
            CreateMap<OwnerWriteDTO, Owner>();
            CreateMap<Owner, OwnersInProjectDTO>();
            CreateMap<OwnerFileImportWriteDTO, OwnerWriteDTO>();

            //Plan
            CreateMap<Plan, PlanReadDTO>();
            CreateMap<Plan, PlanInOwnerReadDTO>()
                .ForMember(des => des.PlanCode, act => act.MapFrom(src => src.PlanCode));
            CreateMap<PlanWriteDTO, Plan>();
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
            CreateMap<LandTypeWriteForImportDTO, LandType>();

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



            //Measured Land Info
            CreateMap<MeasuredLandInfo, MeasuredLandInfoReadDTO>();
            CreateMap<MeasuredLandInfoWriteDTO, MeasuredLandInfo>();

            //Gcn Land Info
            CreateMap<GcnlandInfo, GCNLandInfoReadDTO>();
            CreateMap<GCNLandInfoWriteDTO, GcnlandInfo>();

            //Support
            CreateMap<Support, SupportReadDTO>();
            CreateMap<SupportWriteDTO, Support>();

            //Deduction
            CreateMap<Deduction, DeductionReadDTO>();
            CreateMap<DeductionWriteDTO, Deduction>();

            //AttachFile
            CreateMap<AttachFile, AttachFileReadDTO>();
            CreateMap<AttachFileWriteDTO, AttachFile>();

            //AssetCompensation
            CreateMap<AssetCompensation, AssetCompensationReadDTO>();
            CreateMap<AssetCompensationWriteDTO, AssetCompensation>();

            //GcnLandInfo
            CreateMap<GcnlandInfo, GCNLandInfoReadDTO>();
            CreateMap<GCNLandInfoWriteDTO, GcnlandInfo>();

            //LandPOsitionInfo
            CreateMap<LandPositionInfo, LandPositionInfoReadDTO>();
            CreateMap<LandPositionInfoWriteDTO, LandPositionInfo>();
            CreateMap<LandPositionInfoInProjectWriteDTO, LandPositionInfo>();

            //UnitPriceAsset
            CreateMap<UnitPriceAsset, UnitPriceAssetReadDTO>();
            CreateMap<UnitPriceAssetWriteDTO, UnitPriceAsset>();
            CreateMap<UnitPriceAssetFileImportWriteDTO, UnitPriceAssetWriteDTO>();

            //UnitPriceLand
            CreateMap<UnitPriceLand, UnitPriceLandReadDTO>();
            CreateMap<UnitPriceLandWriteDTO, UnitPriceLand>();
            CreateMap<UnitPriceLandInProjectWriteDTO, UnitPriceLand>();
            CreateMap<UnitPriceLandFileImportWriteDTO, UnitPriceLandWriteDTO>();

            //Resettlement project
            CreateMap<ResettlementProject, ResettlementProjectReadDTO>()
                 .ForMember(des => des.ResettlementDocuments, act => act.MapFrom(src => src.ResettlementDocuments.Select(pd => pd.Document)));
            CreateMap<ResettlementProjectWriteDTO, ResettlementProject>();

            //Land Resettlement
            CreateMap<LandResettlement, LandResettlementReadDTO>();
            CreateMap<LandResettlement, LandResettlementInProjectReadDTO>();
            CreateMap<LandResettlementWriteDTO, LandResettlement>();

        }
    }
}
