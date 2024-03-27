using Factories.WebApi.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Factories.WebApi.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FacilitiesDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}