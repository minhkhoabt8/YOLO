using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AssetCompensation;
using Metadata.Infrastructure.DTOs.AttachFile;
using Metadata.Infrastructure.DTOs.AuditTrail;
using Metadata.Infrastructure.DTOs.Deduction;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.GCNLandInfo;
using Metadata.Infrastructure.DTOs.LandPositionInfo;
using Metadata.Infrastructure.DTOs.MeasuredLandInfo;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Plan;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.Support;
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
        }
    }
}
