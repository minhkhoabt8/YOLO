using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Directory.CreateDirectory(builder.Configuration["StoragePath"]);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = false,
        RequireAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuers = new[] { builder.Configuration["JWT:Issuer"] },
        IssuerSigningKey = new
            SymmetricSecurityKey
            (Encoding.UTF8.GetBytes
                (builder.Configuration["JWT:Secret"]))
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
