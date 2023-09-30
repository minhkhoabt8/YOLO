using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Document;
using Metadata.Infrastructure.DTOs.PriceAppliedCode;
using Metadata.Infrastructure.DTOs.Project;
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
        }
    }
}
