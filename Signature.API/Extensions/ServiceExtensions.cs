using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly.Extensions.Http;
using Signature.Infrastructure.Services.Implementation;
using Signature.Infrastructure.Services.Interfaces;
using System.Reflection;
using System.Text;
using Polly;
using Microsoft.Extensions.DependencyInjection;
using Signature.Infrastructure.Services.Implementations;

namespace Signature.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAuthService, YOLOAuthService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(
                    HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                );
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "YOLO Signature",
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

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDigitalSignatureService, DigitalSignatureService>();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
        }
    }
}
