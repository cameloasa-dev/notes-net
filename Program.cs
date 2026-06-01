using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyOwnApi.Context;
using MyOwnApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authentication + Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();
