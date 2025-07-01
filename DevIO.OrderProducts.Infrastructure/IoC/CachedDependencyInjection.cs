using DevIO.OrderProducts.Application.Interfaces;
using DevIO.OrderProducts.Infrastructure.ExternalServices.Cache;
using DevIO.OrderProducts.Infrastructure.ExternalServices.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.OrderProducts.Infrastructure.IoC
{
    public static class CachedDependencyInjection
    {
        public static IServiceCollection AddCachedDependencies(this IServiceCollection services)
        {
            // Register Redis cache service
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddSingleton<ICachePolicyService, CachePolicyService>();

            return services;
        }
    }
}
