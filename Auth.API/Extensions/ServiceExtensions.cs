using Auth.Core.Data;
using Auth.Infracstructure.Mappers;
using Auth.Infracstructure.Repositories.Implementations;
using Auth.Infracstructure.Repositories.Interfaces;
using Auth.Infracstructure.Services.Implementations;
using Auth.Infracstructure.Services.Interfaces;
using Auth.Infracstructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;


namespace Auth.API.Extensions;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<YoloAuthContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("AuthConnection"),
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
                Title = "YOLO",
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
       
    }

    public static void ApplyPendingMigrations(this IServiceProvider provider)
    {
       
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISampleServices, SampleServices>();
        services.AddScoped<IAccountService, AccountService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
    }

    public static void AddUOW(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddServiceFilters(this IServiceCollection services)
    {
        
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