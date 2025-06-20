using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using URLshortner.Repositories.Interfaces;
using URLshortner.Repositories;
using URLshortner.Services.Interfaces;
using URLshortner.Services;
using URLshortner.Middlewares;
using URLshortner.Filters;
using Microsoft.AspNetCore.Builder;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using ALL.Database;
using URLshortner.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using URLshortner.Utils;
using URLshortner.Dtos.Validators;

namespace URLshortner.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddControllersAndFilters()
                .AddRepositories()
                .AddServices()
                .AddDatabase(configuration)
                .AddEmailService(configuration)
                .AddRedisCache(configuration)
                .AddCustomAuthentication(configuration)
                .AddCookiePolicyOptions()
                .AddCustomAuthorization()
                .AddRateLimitingPolicy()
                .AddExceptionHandling()
                ;
        }

        // Database
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        // Additional Services
        public static IServiceCollection AddAdditionalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();
            return services;
        }

        // Repositories
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserFriendRepository, UserFriendRepository>();
            services.AddScoped<IUrlRepository, UrlRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IActionTokenRepository, ActionTokenRepository>();
            return services;
        }

        // Domain Services
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUrlService, UrlService>();
            services.AddScoped<IUserFriendService, UserFriendService>();
            return services;
        }

        // Email
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentEmail(configuration["EmailSettings:From"])
                    .AddSmtpSender(new SmtpClient
                    {
                        Host = configuration["EmailSettings:SmtpHost"]!,
                        Port = int.Parse(configuration["EmailSettings:SmtpPort"]!),
                        EnableSsl = true,
                        Credentials = new NetworkCredential(
                            configuration["EmailSettings:SmtpUser"],
                            configuration["EmailSettings:SmtpPass"]) }
                    );
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }

        // Redis Cache
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfig = ConfigurationOptions.Parse(configuration["Redis:ConnectionString"]!);
                return ConnectionMultiplexer.Connect(redisConfig);
            });
            return services;
        }

        // Authentication
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            }).AddGoogle(options =>
            {
                        options.ClientId = configuration["Google:ClientId"]!;
                        options.ClientSecret = configuration["Google:ClientSecret"]!;
            });
            return services;
        }

        // Cookie Policy Options
        public static IServiceCollection AddCookiePolicyOptions(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });
            return services;
        }

        // Authorization
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            return services;
        }

        // Rate Limiting
        public static IServiceCollection AddRateLimitingPolicy(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromSeconds(1);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });
            });
            return services;
        }

        // Exception Handling
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }

        // MVC Filters
        public static IServiceCollection AddControllersAndFilters(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
            });
            return services;
        }
    }
}
