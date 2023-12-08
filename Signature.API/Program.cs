using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using SharedLib.Middlewares;
using Signature.API.Extensions;


var builder = WebApplication.CreateBuilder(args);
Directory.CreateDirectory(builder.Configuration["StoragePath"]);

// Add services to the container.

builder.Host.UseSerilog((ctx, lc) =>
{
    if (ctx.HostingEnvironment.IsEnvironment("VPS"))
    {
        lc.WriteTo.File(ctx.Configuration["Logging:FilePath"]).MinimumLevel.Is(LogEventLevel.Error);
    }
    else
    {
        lc.ReadFrom.Configuration(ctx.Configuration);
    }
});

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true
            }
        };
        opt.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddHttpClients();
builder.Services.AddRepositories();
builder.Services.ConfigureSwagger();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
