using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using OcelotGW.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(opt => opt.Limits.MaxRequestBodySize = uint.MaxValue);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
builder.Services.AddCors();
builder.Services.AddOcelot().AddPolly();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddControllers();

builder.Host.UseSerilog((ctx, lc) =>
{
    if (ctx.HostingEnvironment.IsEnvironment("VPS"))
    {
        lc.WriteTo.File(ctx.Configuration["Logging:FilePath"]);
    }
    else
    {
        lc.ReadFrom.Configuration(ctx.Configuration);
    }
});

var app = builder.Build();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<TokenTransformMiddleware>();
app.UseSwagger();
app.UseSwaggerForOcelotUI(opt => { opt.PathToSwaggerGenerator = "/swagger/docs"; });
app.UseRouting();
app.UseCors(opt => opt.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
app.UseWebSockets();
await app.UseOcelot();
app.Run();