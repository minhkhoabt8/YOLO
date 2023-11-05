using Auth.Core.Data;
using Auth.Infrastructure.Services.Implementations;
using Auth.Infrastructure.Repositories.Implementations;
using Auth.Infrastructure.Repositories.Interfaces;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedLib.Filters;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Reflection;
using Auth.Infrastructure.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;

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

    public static void AddFireBase(this IServiceCollection services, IConfiguration configuration)
    {

    }


    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "YOLO Auth",
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
       
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<Infrastructure.Services.Interfaces.ITokenService, Infrastructure.Services.Implementations.JWTTokenService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services
            .AddScoped<SharedLib.Infrastructure.Services.Interfaces.ITokenService,
                SharedLib.Infrastructure.Services.Implementations.JWTTokenService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<Auth.Infrastructure.Services.Interfaces.INotificationService
            , Auth.Infrastructure.Services.Implementations.NotificationService>();
        services.AddScoped<SharedLib.Infrastructure.Services.Interfaces.INotificationService
            , SharedLib.Infrastructure.Services.Implementations.NotificationService>();

    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
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
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
    }

}