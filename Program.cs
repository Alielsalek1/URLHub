using ALL.Database;
using Microsoft.EntityFrameworkCore;
using URLshortner.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Text;
using URLshortner.Utils;
using Microsoft.Extensions.Configuration;
using URLshortner.Middlewares;
using URLshortner.Repositories.Implementations;
using URLshortner.Repositories.Interfaces;
using URLshortner.Models;
using URLshortner.Services.Implementations;
using URLshortner.Services.Interfaces;
using FluentValidation.AspNetCore;
using URLshortner.Dtos.Implementations;
using URLshortner.Dtos.Validators;
using FluentValidation;
using URLshortner.Filters;
using System.Net.Mail;
using System.Net;

// TODO: Unit Testing
// TODO: Task to delete activation tokens
// TODO: Chat with Friends

var builder = WebApplication.CreateBuilder(args);

// Add controllers and their filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});

// Add Database repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFriendRepository, UserFriendRepository>();
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IActivationTokenRepository, ActivationTokenRepository>();

// Add Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUserFriendService, UserFriendService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Add Fluent Email
builder.Services.AddFluentEmail(builder.Configuration["EmailSettings:From"])
    .AddSmtpSender(new SmtpClient
    {
        Host = builder.Configuration["EmailSettings:SmtpHost"],
        Port = int.Parse(builder.Configuration["EmailSettings:SmtpPort"]),
        EnableSsl = true,
        Credentials = new NetworkCredential(
            builder.Configuration["EmailSettings:SmtpUser"],
            builder.Configuration["EmailSettings:SmtpPass"]
        )
    });

// builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Global ExceptionHandler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add JWT Tokens for Authentication
var jwtSettings = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["JwtSettings:Issuer"],
            ValidAudience = jwtSettings["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Jwt:Key"]))
        };
    });

// add Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();