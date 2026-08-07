using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ProcureFlow.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register FluentValidations
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // Register AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
