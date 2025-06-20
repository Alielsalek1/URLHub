using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace URLshortner.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add AutoMapper profiles from Application layer
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add FluentValidation validators from Application layer
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
