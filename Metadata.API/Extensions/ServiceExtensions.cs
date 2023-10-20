using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedLib.Filters;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Metadata.Core.Data;
using Metadata.Infrastructure.UOW;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.Services.Implementations;
using Metadata.Infrastructure.Repositories.Interfaces;
using Metadata.Infrastructure.Repositories.Implementations;
using Metadata.Infrastructure.Mappers;
using SharedLib.Infrastructure.Attributes;

namespace Metadata.API.Extensions;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<YoloMetadataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("MetadataConnection"),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
            );
        });
    }

    public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
       
    }

    public static void AddHttpClients(this IServiceCollection services)
    {
        
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "YOLO Metatdata",
                Version = "v1.0.0",
                Description = "YOLO Project",
            });
            c.UseInlineDefinitionsForEnums();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Add YOLO Bearer Token Here",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        
    }

    public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                RequireAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                IssuerSigningKey = new
                    SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes
                        (configuration["JWT:Secret"]))
            };
        });
    }

    public static void ApplyPendingMigrations(this IServiceProvider provider)
    {
       
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IUploadFileService, UploadFileService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<IAuditTrailService, AuditTrailService>();
        services.AddScoped<IMeasuredLandInfoService,MeasuredLandInfoService>();
        services.AddScoped<IGCNLandInfoService,GCNLandInfoService>();
        services.AddScoped<ISupportService,SupportService>();
        services.AddScoped<IDeductionService, DeductionService>();
        services.AddScoped<IAssetCompensationService, AssetCompensationService>();
        services.AddScoped<IAttachFileService, AttachFileService>();

    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IDocumentRepository,DocumentRepository>();
        services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IEntityAuditor, EntityAuditor>();
        services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
        services.AddScoped<IMeasuredLandInfoRepository,MeasuredLandInfoRepository>();
        services.AddScoped<IGCNLandInfoRepository, GCNLandInfoRepository>();
        services.AddScoped<ISupportRepository,SupportRepository>();
        services.AddScoped<IDeductionRepository, DeductionRepository>();
        services.AddScoped<IAssetCompensationRepository, AssetCompensationRepository>();
        services.AddScoped<IAttachFileRepository, AttachFileRepository>();
    }

    public static void AddUOW(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddServiceFilters(this IServiceCollection services)
    {
        services.AddScoped<AutoValidateModelState>();
    }

    public static void AddMassTransit(this IServiceCollection services, IConfiguration conf)
    {
       
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfiles)));
    }

    public static void AddEvents(this IServiceCollection services)
    {
       
    }

    public static void ConfigureApiOptions(this IServiceCollection services)
    {
        
    }
}