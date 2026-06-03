using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using MyOwnApi.Context;
using MyOwnApi.DTOs;
using MyOwnApi.Models;
using MyOwnApi.Repositories;
using MyOwnApi.Services;
using MyOwnApi.Validators;
using MyOwnApi.Middleware;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// Controllers + FluentValidation
// ---------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// ---------------------------------------------------------
// DbContext
// ---------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ---------------------------------------------------------
// Identity
// ---------------------------------------------------------
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ---------------------------------------------------------
// Rate Limiting
// ---------------------------------------------------------
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("notesLimiter", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });
});

// ---------------------------------------------------------
// Repositories
// ---------------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();

// ---------------------------------------------------------
// Services
// ---------------------------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<INotesService, NotesService>();

// ---------------------------------------------------------
// Validators
// ---------------------------------------------------------
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<ApiKeyValidator>();

// ---------------------------------------------------------
// Authentication + Authorization
// ---------------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

builder.Services.AddAuthorization();

// ---------------------------------------------------------
// Swagger
// ---------------------------------------------------------
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access protected endpoints. Format: X-Api-Key: {your_key}",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// ---------------------------------------------------------
// Pipeline
// ---------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRateLimiter();
}

app.UseAuthentication();
app.UseAuthorization();

// Custom API Key Middleware
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
