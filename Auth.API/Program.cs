
using Auth.API.Extensions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using SharedLib.Middlewares;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddHttpClients();
builder.Services.AddServiceFilters();
builder.Services.AddRepositories();
builder.Services.AddMassTransit(builder.Configuration);
builder.Services.AddUOW();
builder.Services.AddAutoMapper();
builder.Services.AddEvents();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureApiOptions();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Services.ApplyPendingMigrations();

app.Run();