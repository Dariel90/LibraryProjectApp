using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Data;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryAPI.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            // Inyectar el servicio de Albumes
            services.AddTransient<ILibraryRepository, LibraryRepository>();

            return services;
        }
    }
}
