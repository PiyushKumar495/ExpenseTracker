using FinTrack.Application.Configuration;
using FinTrack.Application.Interfaces;
using FinTrack.Infrastructure.Security;
using FinTrack.Infrastructure.Persistence.Repositories;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using FinTrack.Application.Features.Authentication;
using FinTrack.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinTrackDbContext>(options=>options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSetttings"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();


